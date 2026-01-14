using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Features.Inscription.ViewModels;
using systeme_gestion_isga.Infrastructure.UnitOfWork;

namespace systeme_gestion_isga.Features.Inscription.Controllers
{
    public class InscriptionController : Controller
    {
        private readonly IUnitOfWork _uow;

        public InscriptionController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public InscriptionController()
        {
            _uow = new UnitOfWork();
        }

        // ============================
        // INDEX
        // ============================
        public ActionResult Index()
        {
            var list = _uow.Inscriptions
                .GetAll()
                .Select(i =>
                {
                    Debug.WriteLine("==== Inscription ====");
                    Debug.WriteLine($"Id: {i.Id}");
                    Debug.WriteLine($"Level: {i.Level?.Name}");
                    Debug.WriteLine($"Student: {i.Student?.User?.FirstName} {i.Student?.User?.LastName}");
                    Debug.WriteLine("----------------------");

                    return new InscriptionVM
                    {
                        Id = i.Id,

                        // IDs (not used in index display but nice to keep)
                        LevelId = i.LevelId,
                        AcademicYearId = _uow.ProgramAcademicYears.GetAll().Where(p => p.Id == i.Level.ProgramAcademicYearId).First().AcademicYearId,
                        ProgramAcademicYearId = _uow.ProgramAcademicYears.GetAll().Where(p => p.Id == i.Level.ProgramAcademicYearId).First().Id,
                        StudentId = i.StudentId,
                        StudentName = i.Student.User.FirstName + " " + i.Student.User.LastName,
                        InscriptionDate = i.InscriptionDate,

                        // If you want display strings in Index, you can either:
                        // 1) add extra props in InscriptionVM like AcademicYearName etc.
                        // 2) or keep Index view reading navigation props.
                        //
                        // For now, keep it simple: you can show in view using navigation
                        // OR add these fields to VM if you prefer.
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
            var vm = new InscriptionVM();
            FillDropdowns(vm);
            return View(vm);
        }

        // ============================
        // CREATE (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InscriptionVM model)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns(model);
                return View(model);
            }

            var entity = new Domain.Entities.Inscription
            {
                //AcademicYearId = model.AcademicYearId.Value,
                //ProgramAcademicYearId = model.ProgramAcademicYearId.Value,
                LevelId = model.LevelId.Value,
                InscriptionDate = model.InscriptionDate,
                StudentId = model.StudentId.Value,
                CreatedAt = DateTime.Now
            };

            _uow.Inscriptions.Insert(entity);
            _uow.Save();

            return RedirectToAction("Index");
        }

        // ============================
        // EDIT (GET)
        // ============================
        public ActionResult Edit(int id)
        {
            var entity = _uow.Inscriptions.GetById(id);
            if (entity == null) return HttpNotFound();

            var vm = new InscriptionVM
            {
                Id = entity.Id,
                AcademicYearId = _uow.ProgramAcademicYears.GetAll().Where(p => p.Id == entity.Level.ProgramAcademicYearId).First().AcademicYearId,
                ProgramAcademicYearId = _uow.ProgramAcademicYears.GetAll().Where(p => p.Id == entity.Level.ProgramAcademicYearId).First().Id,
                InscriptionDate = entity.InscriptionDate,
                LevelId = entity.LevelId,
                StudentId = entity.StudentId
            };

            FillDropdowns(vm);
            return View(vm);
        }

        // ============================
        // EDIT (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InscriptionVM model)
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns(model);
                return View(model);
            }

            var entity = _uow.Inscriptions.GetById(model.Id);
            if (entity == null) return HttpNotFound();

            entity.LevelId = model.LevelId.Value;
            entity.StudentId = model.StudentId.Value;

            // optional if you have it
            entity.UpdatedAt = DateTime.Now;

            _uow.Inscriptions.Update(entity);
            _uow.Save();

            return RedirectToAction("Index");
        }

        // ============================
        // DELETE (GET)
        // ============================
        public ActionResult Delete(int id)
        {
            var entity = _uow.Inscriptions.GetById(id);
            if (entity == null) return HttpNotFound();

            var vm = new InscriptionVM
            {
                Id = entity.Id,
                AcademicYearId = _uow.ProgramAcademicYears.GetAll().Where(p => p.Id == entity.Level.ProgramAcademicYearId).First().AcademicYearId,
                ProgramAcademicYearId = _uow.ProgramAcademicYears.GetAll().Where(p => p.Id == entity.Level.ProgramAcademicYearId).First().Id,
                InscriptionDate = entity.InscriptionDate,
                LevelId = entity.LevelId,
                StudentId = entity.StudentId
            };

            // if your delete page needs dropdown texts, fill them
            FillDropdowns(vm);

            return View(vm);
        }

        // ============================
        // DELETE CONFIRMED (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var entity = _uow.Inscriptions.GetById(id);
            if (entity == null)
                return RedirectToAction("Index");

            try
            {
                _uow.Inscriptions.Delete(entity.Id);
                _uow.Save();
                TempData["Success"] = "Inscription deleted successfully.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Delete Inscription failed: " + ex);
                TempData["Error"] = ex.GetBaseException().Message;
            }

            return RedirectToAction("Index");
        }

        // ============================
        // HELPERS
        // ============================
        private void FillDropdowns(InscriptionVM vm)
        {
            // Academic Years
            vm.AcademicYears = _uow.AcademicYears
                .GetAll()
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                })
                .ToList();

            // Programs in year (ProgramAcademicYear)
            // I recommend Text = "ProgramName (AcademicYearName)" or just ProgramName
            vm.ProgramAcademicYears = _uow.ProgramAcademicYears
                .GetAll()
                .Select(pay => new SelectListItem
                {
                    Value = pay.Id.ToString(),
                    Text = pay.Program.Name
                })
                .ToList();

            // Levels
            vm.Levels = _uow.Levels
                .GetAll()
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToList();

            // Students
            // Adjust if your student entity isn't Student.User
            vm.Students = _uow.Students
                .GetAll()
                .Select(s => new SelectListItem
                {
                    Value = s.UserID.ToString(), // or s.Id depending on your schema
                    Text = s.User.FirstName + " " + s.User.LastName
                })
                .ToList();

            // ============================
            // Cascading dictionaries for Create.cshtml filtering
            // ============================

            // ProgramAcademicYearId -> AcademicYearId
            ViewBag.ProgramAcademicYearToAcademicYear = _uow.ProgramAcademicYears
                .GetAll()
                .ToDictionary(
                    pay => pay.Id,
                    pay => pay.AcademicYearId
                );

            // LevelId -> ProgramAcademicYearId
            ViewBag.LevelToProgramAcademicYear = _uow.Levels
                .GetAll()
                .ToDictionary(
                    l => l.Id,
                    l => l.ProgramAcademicYearId
                );
        }
    }
}
