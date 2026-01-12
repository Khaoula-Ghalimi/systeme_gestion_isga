using System;
using System.Collections.Generic;
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
            var academicYears= _uow.AcademicYears
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
            var academicYear = _uow.AcademicYears.GetById(id);
            var programs_input = _uow.Programs.GetAll().Select(p => new ProgramVM
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                DurationInYears = p.DurationInYears

            }).OrderBy(p => p.Id).ToList();
            
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
            if(academicYear == null)
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
    }
}