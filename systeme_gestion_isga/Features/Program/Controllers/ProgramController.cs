using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Features.Program.ViewModels;
using systeme_gestion_isga.Infrastructure.Repositories.Levels;
using systeme_gestion_isga.Infrastructure.Repositories.Programs;
using systeme_gestion_isga.Infrastructure.Repositories.Semesters;
using systeme_gestion_isga.Infrastructure.UnitOfWork;

namespace systeme_gestion_isga.Features.Program.Controllers
{
    public class ProgramController : Controller
    {

        private readonly IUnitOfWork _uow;


        public ProgramController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ProgramController()
        {
            _uow = new UnitOfWork();
        }




        // GET: Program
        public ActionResult Index()
        {
            var programs = _uow.Programs
                .GetAll()
                .Select(p => new ProgramVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Description = p.Description,
                    DurationInYears = p.DurationInYears,
                })
                .OrderBy(p => p.Id)
                .ToList();
            return View(programs);
        }

        //GET : Program/Create
        public ActionResult Create()
        {
            return View(new ProgramVM());
        }

        //POST : Program/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProgramVM model)
        {
            

            if (!ModelState.IsValid)
                return View(model);

            var program = new Domain.Entities.Program
            {
                Name = model.Name,
                Code = model.Code,
                Description = model.Description,
                DurationInYears = model.DurationInYears,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _uow.Programs.Insert(program);
            _uow.Programs.Save();

            
            return RedirectToAction("Index");

        }
        // GET : Program/Edit/5
        public ActionResult Edit(int id)
        {
            var program = _uow.Programs.GetById(id);
            if (program == null)
                return HttpNotFound();
         
            var model = new ProgramVM
            {
                Id = id,
                Name = program.Name,
                Code = program.Code,
                Description = program.Description,
                DurationInYears = program.DurationInYears,
            };
            return View(model);
        }
        //POST : Program/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProgramVM model)
        {
            
            if (!ModelState.IsValid)
                return View(model);
            var program = _uow.Programs.GetById(model.Id);
            if (program == null) return HttpNotFound();
            program.Name = model.Name;
            program.Code = model.Code;
            program.Description = model.Description;
            program.DurationInYears = model.DurationInYears;
            _uow.Programs.Update(program);
            _uow.Programs.Save();

            return RedirectToAction("Index");
        }
        //GET : Program/Delete/5
        public ActionResult Delete(int id)
        {
            var program = _uow.Programs.GetById(id);
            if (program == null)
                return HttpNotFound();
            var model = new ProgramVM
            {
                Id = program.Id,
                Name = program.Name,
                Code = program.Code,
                Description = program.Description,
                DurationInYears = program.DurationInYears
            };
            return View(model);
        }

        //POST : Program/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var program = _uow.Programs.GetById(id);
            if (program == null)
                return HttpNotFound();
            _uow.Programs.Delete(program.Id);
            _uow.Programs.Save();
            return RedirectToAction("Index");
        }
    }
}