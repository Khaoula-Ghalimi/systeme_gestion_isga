using System.Linq;
using System.Web.Mvc;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Features.Semester.ViewModels;
using systeme_gestion_isga.Infrastructure.UnitOfWork;

namespace systeme_gestion_isga.Features.Semester.Controllers
{
    public class SemesterController : Controller
    {
        private readonly IUnitOfWork _uow;

        public SemesterController(IUnitOfWork uow) { _uow = uow; }
        public SemesterController() { _uow = new UnitOfWork(); }

        // GET: Semester
        public ActionResult Index()
        {
            // 1) Load all base lists once (avoid N+1 hell)
            var academicYears = _uow.AcademicYears.GetAll()
                .OrderByDescending(a => a.StartDate)
                .ToList();

            var pays = _uow.ProgramAcademicYears.GetAll().ToList();
            var levels = _uow.Levels.GetAll().ToList();
            var semesters = _uow.Semesters.GetAll().ToList();

            // 2) Build the full tree in memory
            var vm = new AcademicYearsTreeVM
            {
                AcademicYears = academicYears.Select(ay => new AcademicYearNodeVM
                {
                    AcademicYearId = ay.Id,
                    AcademicYearName = ay.Name,
                    StartDate = ay.StartDate,
                    EndDate = ay.EndDate,
                    IsActive = ay.IsActive,

                    Programs = pays
                        .Where(p => p.AcademicYearId == ay.Id)
                        .Select(p => new ProgramNodeVM
                        {
                            ProgramAcademicYearId = p.Id,
                            ProgramId = p.ProgramId,

                            // Overrides if you have them:
                            //ProgramName = string.IsNullOrWhiteSpace(p.DisplayName) ? p.Program.Name : p.DisplayName,
                            //ProgramCode = string.IsNullOrWhiteSpace(p.DisplayCode) ? p.Program.Code : p.DisplayCode,
                            DurationInYears = p.DurationInYearsOverride ?? p.Program.DurationInYears,
                            IsActive = p.IsActive,

                            Levels = levels
                                .Where(l => l.ProgramAcademicYearId == p.Id)
                                .OrderBy(l => l.Order)
                                .Select(l => new LevelNodeVM
                                {
                                    LevelId = l.Id,
                                    LevelName = l.Name,
                                    Order = l.Order,

                                    Semesters = semesters
                                        .Where(s => s.LevelId == l.Id)
                                        .OrderBy(s => s.Order)
                                        .Select(s => new SemesterNodeVM
                                        {
                                            SemesterId = s.Id,
                                            Name = s.Name,
                                            Order = s.Order
                                        })
                                        .ToList()
                                })
                                .ToList()
                        })
                        .OrderBy(p => p.ProgramName)
                        .ToList()
                }).ToList()
            };

            return View(vm);
        }


        public ActionResult Edit(int id)
        {
            var semester = _uow.Semesters.GetById(id);
            if (semester == null) return HttpNotFound();

            var vm = new SemesterEditModulesVM
            {
                SemesterId = semester.Id,
                SemesterName = semester.Name,
                SemesterOrder = semester.Order,

                SemesterModules = semester.SemesterModules
                    .Select(sm => new SemesterModuleVM
                    {
                        Id = sm.Id,
                        ModuleId = sm.ModuleId,
                        ModuleName = sm.Module.Name,
                        ModuleCode = sm.Module.Code
                    })
                    .ToList()
            };

            vm.AvailableModules = _uow.Modules.GetAll()
                .Select(m => new ModuleLookupVM
                {
                    Id = m.Id,
                    Name = $"{m.Code} : {m.Name}"
                })
                .OrderBy(x => x.Name)
                .ToList();

            return View(vm);
        }

        // POST: Semester/EditModules
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SemesterEditModulesVM model)
        {
            var semester = _uow.Semesters.GetById(model.SemesterId);
            if (semester == null) return HttpNotFound();

            // refill dropdown if invalid
            if (!ModelState.IsValid)
            {
                model.AvailableModules = _uow.Modules.GetAll()
                    .Select(m => new ModuleLookupVM
                    {
                        Id = m.Id,
                        Name = $"{m.Code} : {m.Name}"
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                // keep names for header
                model.SemesterName = semester.Name;
                model.SemesterOrder = semester.Order;

                return View(model);
            }

            // submitted module IDs
            var newModuleIds = (model.SemesterModules ?? new System.Collections.Generic.List<SemesterModuleVM>())
                .Select(x => x.ModuleId)
                .Distinct()
                .ToList();

            // existing pivots in DB
            var existingPivots = _uow.SemesterModules.GetAll()
                .Where(sm => sm.SemesterId == semester.Id)
                .ToList();

            var existingModuleIds = existingPivots.Select(x => x.ModuleId).ToList();

            // 1) remove deleted modules
            var toRemove = existingPivots.Where(x => !newModuleIds.Contains(x.ModuleId)).ToList();
            foreach (var pivot in toRemove)
                _uow.SemesterModules.Delete(pivot.Id);

            // 2) add newly selected modules
            var toAdd = newModuleIds.Where(mid => !existingModuleIds.Contains(mid)).ToList();
            foreach (var moduleId in toAdd)
            {
                _uow.SemesterModules.Insert(new SemesterModule
                {
                    SemesterId = semester.Id,
                    ModuleId = moduleId
                });
            }

            _uow.SemesterModules.Save();

            // back to tree (adjust controller/action name if different)
            return RedirectToAction("Index", "Semester");
        }
    }
}
