using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using systeme_gestion_isga.Features.AcademicYear.ViewModels;
using systeme_gestion_isga.Features.Program.ViewModels;
using systeme_gestion_isga.Infrastructure.Repositories.AcademicYears;
using systeme_gestion_isga.Infrastructure.UnitOfWork;

namespace systeme_gestion_isga.Features.AcademicYear.Controllers
{
    public class AcademicYearController : Controller
    {


        private readonly IUnitOfWork _uow;

        public AcademicYearController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public AcademicYearController()
        {
            _uow = new UnitOfWork();
        }

        // GET: AcademicYear
        public ActionResult Index()
        {
            var academicYears = _uow.AcademicYears
                .GetAll()
                .Select(a => new AcademicYearVM
                {
                    Id = a.Id,
                    Name = a.Name,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    IsActive = a.IsActive
                })
                .OrderBy(a => a.Id)
                .ToList();
            return View(academicYears);
        }

        // GET: AcademicYear/Details/5
        public ActionResult Details(int id)
        {
            Debug.WriteLine("Hmmm : " + _uow.Levels.GetByProgramAcademicYearId(5)[0].Code);
            var academicYear = _uow.AcademicYears.GetById(id);
            var programs = _uow.ProgramAcademicYears.GetByAcademicYear(id);
            var programs_excluded = _uow.ProgramAcademicYears.GetExceptAcademicYear(id);

            
            if (academicYear == null)
                return HttpNotFound();
            var model = new AcademicYearVM
            {
                Id = academicYear.Id,
                Name = academicYear.Name,
                StartDate = academicYear.StartDate,
                EndDate = academicYear.EndDate,
                IsActive = academicYear.IsActive,
                Programs = programs.Select(p =>
                {
                    // <--- IMPORTANT
                    Debug.WriteLine("Hmmm : " + p.Id);
                    return new ProgramAcademicYearVM
                    {
                        Id = p.Id,
                        ProgramId = p.ProgramId,
                        Name = p.Name,
                        Code = p.Code,
                        DurationInYearsOverride = p.DurationInYearsOverride,
                        IsActive = p.IsActive,

                        Levels = _uow.Levels.GetByProgramAcademicYearId(p.Id)
                                    .Select(l => new LevelVM
                                    {
                                        Name = l.Name,
                                        Code = l.Code,
                                        Order = l.Order
                                    }).ToList()
                    };
                }).ToList(),
                ExcludedPrograms = programs_excluded.Select(p => new Features.AcademicYear.ViewModels.ProgramVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    DurationInYears = p.DurationInYears
                }).ToList()
            };
            return View(model);
        }

        // GET: AcademicYear/Create
        public ActionResult Create()
        {
            return View(new AcademicYearVM());
        }

        // POST: AcademicYear/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AcademicYearVM model)
        {
            if (ModelState.IsValid)
            {
                var academicYear = new Domain.Entities.AcademicYear
                {
                    Name = model.Name,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsActive = model.IsActive
                };
                _uow.AcademicYears.Insert(academicYear);
                _uow.AcademicYears.Save();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //GET : AcademicYear/Edit/5
        public ActionResult Edit(int id)
        {
            var academicYear = _uow.AcademicYears.GetById(id);
            if (academicYear == null)
                return HttpNotFound();

            var model = new AcademicYearVM
            {
                Id = academicYear.Id,
                Name = academicYear.Name,
                StartDate = academicYear.StartDate,
                EndDate = academicYear.EndDate,
                IsActive = academicYear.IsActive
            };
            return View(model);

        }
        //POST : AcademicYear/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AcademicYearVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var academicYear = _uow.AcademicYears.GetById(model.Id);
            if (academicYear == null) return HttpNotFound();
            academicYear.Name = model.Name;
            academicYear.StartDate = model.StartDate;
            academicYear.EndDate = model.EndDate;
            academicYear.IsActive = model.IsActive;
            _uow.AcademicYears.Update(academicYear);
            _uow.AcademicYears.Save();
            return RedirectToAction("Index");
        }

        //GET : AcademicYear/Delete/5
        public ActionResult Delete(int id)
        {
            var academicYear = _uow.AcademicYears.GetById(id);
            if (academicYear == null)
                return HttpNotFound();

            var model = new AcademicYearVM
            {
                Id = academicYear.Id,
                Name = academicYear.Name,
                StartDate = academicYear.StartDate,
                EndDate = academicYear.EndDate,
                IsActive = academicYear.IsActive
            };
            return View(model);
        }

        //POST : AcademicYear/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var academicYear = _uow.AcademicYears.GetById(id);
            if (academicYear == null)
                return HttpNotFound();
            _uow.AcademicYears.Delete(academicYear.Id);
            _uow.AcademicYears.Save();
            return RedirectToAction("Index");
        }

        //POST : AcademicYear/AddProgramToYear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProgramToYear(AcademicYearVM model)
        {
            
            if (model?.ProgramAcademicYearVM == null)
                return RedirectToAction("Details", new { id = model.Id });

            var vm = model.ProgramAcademicYearVM;
            Debug.WriteLine("hmmm : "+model.ProgramAcademicYearVM.Id);


            // years
            var years = vm.DurationInYearsOverride;
            if (years <= 0)
            {
                var program = _uow.Programs.GetById(vm.ProgramId);
                years = program?.DurationInYears ?? 0;
            }
            if (years <= 0)
                return RedirectToAction("Details", new { id = model.Id });

            // EDIT MODE?
            if (vm.IsEdit) // bool in VM, or use hidden "isEditMode"
            {
                Debug.WriteLine(model.Id);
                return EditProgramInYear(model.Id, vm, years);
            }

            // CREATE MODE (your current logic)
            var pay = new Domain.Entities.ProgramAcademicYear
            {
                AcademicYearId = model.Id,
                ProgramId = vm.ProgramId,
                DurationInYearsOverride = vm.DurationInYearsOverride,
                IsActive = true
            };

            _uow.ProgramAcademicYears.Insert(pay);
            _uow.Save(); // ensures pay.Id

            RecreateLevelsAndSemesters(pay.Id, years, vm.Levels);


            return RedirectToAction("Details", new { id = model.Id });
        }



        private ActionResult EditProgramInYear(int academicYearId, ProgramAcademicYearVM vm, int years)
        {
            Debug.WriteLine("called1 : ");
            // 1) load the join row by its ID
            var pay = _uow.ProgramAcademicYears.GetById(vm.Id);
            if (pay == null) return HttpNotFound();
            Debug.WriteLine("called2");
            // optional safety check
            if (pay.AcademicYearId != academicYearId) return HttpNotFound();

            // 2) update row fields
            pay.DurationInYearsOverride = vm.DurationInYearsOverride;
            // pay.IsActive = vm.IsActive; // if you want to allow status edit here
            _uow.ProgramAcademicYears.Update(pay);
            _uow.Save();

            // 3) delete old semesters + levels, then recreate
            DeleteLevelsAndSemesters(pay.Id);
            RecreateLevelsAndSemesters(pay.Id, years, vm.Levels);

            return RedirectToAction("Details", new { id = academicYearId });
        }

        private void DeleteLevelsAndSemesters(int programAcademicYearId)
        {
            // get levels of this join row
            var levels = _uow.Levels.GetByProgramAcademicYearId(programAcademicYearId);

            foreach (var level in levels)
            {
                // delete semesters of this level first
                var semesters = _uow.Semesters.GetAll().Where(s => s.LevelId == level.Id).ToList();
                foreach (var sem in semesters)
                    _uow.Semesters.Delete(sem.Id);

                _uow.Save();

                // delete the level
                _uow.Levels.Delete(level.Id);
                _uow.Save();
            }
        }

        private void RecreateLevelsAndSemesters(int programAcademicYearId, int years, List<LevelVM> postedLevels)
        {
            for (int i = 0; i < years; i++)
            {
                var lvlVm = (postedLevels != null && postedLevels.Count > i) ? postedLevels[i] : null;

                var level = new Domain.Entities.Level
                {
                    ProgramAcademicYearId = programAcademicYearId,
                    Order = i + 1,
                    Name = !string.IsNullOrWhiteSpace(lvlVm?.Name) ? lvlVm.Name : $"Year {i + 1}",
                    Code = !string.IsNullOrWhiteSpace(lvlVm?.Code) ? lvlVm.Code : $"L{i + 1}",
                };

                _uow.Levels.Insert(level);
                _uow.Save(); // ensures level.Id

                var s1 = new Domain.Entities.Semester
                {
                    LevelId = level.Id,
                    Order = 1,
                    Name = "Semester 1"
                };

                var s2 = new Domain.Entities.Semester
                {
                    LevelId = level.Id,
                    Order = 2,
                    Name = "Semester 2"
                };

                _uow.Semesters.Insert(s1);
                _uow.Semesters.Insert(s2);
                _uow.Save();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ToggleProgramActive(int academicYearId, int programId)
        {
            // get the ProgramAcademicYear row (the join table)
            var row = _uow.ProgramAcademicYears
                         .GetAll()
                         .FirstOrDefault(x => x.AcademicYearId == academicYearId && x.ProgramId == programId);

            if (row == null) return HttpNotFound();

            row.IsActive = !row.IsActive;

            _uow.Save(); 

            return RedirectToAction("Details", new { id = academicYearId });
        }

    }
}