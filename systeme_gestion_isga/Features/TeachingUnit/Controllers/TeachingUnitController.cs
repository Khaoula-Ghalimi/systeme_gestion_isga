using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Features.Module.ViewModels;
using systeme_gestion_isga.Features.TeachingUnit.ViewModels;
using systeme_gestion_isga.Infrastructure.UnitOfWork;

namespace systeme_gestion_isga.Features.TeachingUnit.Controllers
{
    public class TeachingUnitController : Controller
    {
        private readonly IUnitOfWork _uow;

        public TeachingUnitController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public TeachingUnitController()
        {
            _uow = new UnitOfWork();
        }

        // ============================
        // INDEX
        // ============================
        public ActionResult Index()
        {
            var list = _uow.TeachingUnits
                .GetAll()
                .Select(t =>
                {
                    Debug.WriteLine("==== TeachingUnit ====");
                    Debug.WriteLine($"TU Id: {t.Id}");

                    Debug.WriteLine($"AcademicYear: {t.AcademicYear?.Name}");
                    Debug.WriteLine($"Level: {_uow.Levels.GetById(t.Semester.LevelId).Name}");
                    Debug.WriteLine($"Semester: {t.Semester?.Name}");
                    Debug.WriteLine($"Teacher: {t.Teacher?.User?.FirstName} {t.Teacher?.User?.LastName}");
                    Debug.WriteLine($"Module: {t.ModuleSubject?.Module?.Name}");
                    Debug.WriteLine("----------------------");

                    // Return the VM
                    return new TeachingUnitVM
                    {
                        Id = t.Id,
                        AcademicYearName = t.AcademicYear.Name,
                        LevelName = _uow.Levels.GetById(t.Semester.LevelId).Name,
                        SemesterName = t.Semester.Name,
                        TeacherName = t.Teacher.User.FirstName + " " + t.Teacher.User.LastName,
                        ModuleMatiereName = t.ModuleSubject.Module.Name
                    };
                })
                .ToList();

            return View(list);
        }


        // ============================
        // CREATE (GET)
        // ============================
        public ActionResult Create()
        {
            var vm = new TeachingUnitVM();
            FillDropdowns(vm);
            return View(vm);
        }

        // ============================
        // CREATE (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeachingUnitVM model)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns(model);
                return View(model);
            }


            var entity = new Domain.Entities.TeachingUnit
            {
                AcademicYearId = model.AcademicYearId,
                SemesterId = model.SemesterId,
                TeacherId = model.TeacherId,
                ModuleSubjectId = model.ModuleMatiereId,
                CreatedAt = DateTime.Now
            };

            _uow.TeachingUnits.Insert(entity);
            _uow.Save();

            return RedirectToAction("Index");
        }

        // ============================
        // HELPERS
        // ============================
        private void FillDropdowns(TeachingUnitVM vm)
        {
            vm.AcademicYears = _uow.AcademicYears
                .GetAll()
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();

            vm.Levels = _uow.Levels
                .GetAll()
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                }).ToList();

            vm.Semesters = _uow.Semesters
                .GetAll()
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList();

            vm.Teachers = _uow.Teachers
                .GetAll()
                .Select(t => new SelectListItem
                {
                    Value = t.UserID.ToString(),
                    Text = t.User.FirstName + " " + t.User.LastName
                }).ToList();

            vm.ModuleMatieres = _uow.ModuleSubjects
                .GetAll()
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Subject.Name
                }).ToList();


            ViewBag.LevelToAcademicYear = _uow.Levels
            .GetAll()
            .ToDictionary(
                l => l.Id,
                l => l.ProgramAcademicYear.AcademicYearId // <-- adjust if your schema differs
            );

            // Semester -> Level mapping
            ViewBag.SemesterToLevel = _uow.Semesters
                .GetAll()
                .ToDictionary(
                    s => s.Id,
                    s => s.LevelId
                );

            ViewBag.ModuleSubjectToSemesters = _uow.SemesterModules
            .GetAll()
            .GroupBy(sm => sm.ModuleId)
            .ToDictionary(
                g => g.Key,                       // ModuleId
                g => g.Select(x => x.SemesterId)   // semesters that contain this module
                      .Distinct()
                      .ToList()
            );
            ViewBag.ModuleSubjectToModule = _uow.ModuleSubjects
            .GetAll()
            .ToDictionary(ms => ms.Id, ms => ms.ModuleId);

        }


        public ActionResult Edit(int id)
        {
            var entity = _uow.TeachingUnits.GetById(id);
            if (entity == null) return HttpNotFound();

            var vm = new TeachingUnitVM
            {
                Id = entity.Id,
                AcademicYearId = entity.AcademicYearId,
                SemesterId = entity.SemesterId,
                LevelId = entity.Semester.LevelId, // derived from semester
                TeacherId = entity.TeacherId,
                ModuleMatiereId = entity.ModuleSubjectId
            };

            FillDropdowns(vm);
            return View(vm);
        }

        // ============================
        // EDIT (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeachingUnitVM model)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns(model);
                return View(model);
            }

            var entity = _uow.TeachingUnits.GetById(model.Id);
            if (entity == null) return HttpNotFound();

            entity.AcademicYearId = model.AcademicYearId;
            entity.SemesterId = model.SemesterId;
            entity.TeacherId = model.TeacherId;
            entity.ModuleSubjectId = model.ModuleMatiereId;

            // optional:
            entity.UpdatedAt = DateTime.Now; // only if you have this field

            _uow.TeachingUnits.Update(entity);
            _uow.Save();

            return RedirectToAction("Index");
        }


        // ============================
        // DELETE (GET)  (optional)
        // ============================
        // You don't *need* this if you only delete via modal,
        // but it's useful if you ever want a dedicated delete page.
        public ActionResult Delete(int id)
        {
            var entity = _uow.TeachingUnits.GetById(id);
            if (entity == null) return HttpNotFound();
            Debug.WriteLine(id);
            // Optional: reuse Edit VM for display, or create a small Delete VM if you prefer.
            var vm = new TeachingUnitVM
            {
                Id = entity.Id,
                AcademicYearName = entity.AcademicYear?.Name,
                LevelName = _uow.Levels.GetById(entity.Semester.LevelId)?.Name,
                SemesterName = entity.Semester?.Name,
                TeacherName = entity.Teacher?.User?.FirstName + " " + entity.Teacher?.User?.LastName,
                ModuleMatiereName = entity.ModuleSubject?.Module?.Name
            };

            return View(vm);
        }


        // ============================
        // DELETE CONFIRMED (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Debug.WriteLine(id);
            var entity = _uow.TeachingUnits.GetById(id);
            if (entity == null)
                return RedirectToAction("Index");

            try
            {
                _uow.TeachingUnits.Delete(entity.Id);   // if your repo expects entity
                                                     // OR: _uow.TeachingUnits.Delete(id); // if your repo expects id
                _uow.Save();

                TempData["Success"] = "Teaching Unit deleted successfully.";
            }
            catch (Exception ex)
            {
                // Most common: FK constraint (this teaching unit is referenced elsewhere)
                Debug.WriteLine("Delete TeachingUnit failed: " + ex);

                TempData["Error"] = ex.GetBaseException().Message;
            }

            return RedirectToAction("Index");
        }

    }





}
