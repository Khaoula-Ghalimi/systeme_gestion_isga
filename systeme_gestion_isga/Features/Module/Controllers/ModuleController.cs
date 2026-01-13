using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Features.Module.ViewModels;
using systeme_gestion_isga.Infrastructure.UnitOfWork;

namespace systeme_gestion_isga.Features.Module.Controllers
{
    public class ModuleController : Controller
    {
        private readonly IUnitOfWork _uow;

        public ModuleController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ModuleController()
        {
            _uow = new UnitOfWork();
        }

        // GET: Module
        public ActionResult Index()
        {
            var modules = _uow.Modules
                .GetAll()
                .Select(m => new ModuleVM
                {
                    Id = m.Id,
                    Name = m.Name,
                    Code = m.Code,
                })
                .OrderBy(m => m.Id)
                .ToList();
            



            return View(modules);
        }

        // GET: Module/Create
        public ActionResult Create()
        {
            var vm = new ModuleVM();
            vm.AvailableSubjects = _uow.Subjects.GetAll()
                .Select(s => new SubjectLookupVM
                {
                    Id = s.Id,
                    Name = $"{s.Code} : {s.Name}"
                })
                .OrderBy(x => x.Name)
                .ToList();

            return View(vm);
        }


        // POST: Module/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModuleVM model)
        {
            if (!ModelState.IsValid)
            {
                // Refill dropdown if validation fails
                model.AvailableSubjects = _uow.Subjects.GetAll()
                    .Select(s => new SubjectLookupVM
                    {
                        Id = s.Id,
                        Name = $"{s.Code} : {s.Name}"
                    })
                    .ToList();

                return View(model);
            }

            // 1️⃣ Create module
            var module = new systeme_gestion_isga.Domain.Entities.Module
            {
                Name = model.Name,
                Code = model.Code,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _uow.Modules.Insert(module);
            _uow.Modules.Save(); // so module.Id is generated

            // 2️⃣ Create pivot rows (ModuleSubject)
            if (model.ModuleSubjects != null && model.ModuleSubjects.Any())
            {
                foreach (var s in model.ModuleSubjects)
                {
                    var pivot = new ModuleSubject
                    {
                        ModuleId = module.Id,
                        SubjectId = s.SubjectId
                    };

                    _uow.ModuleSubjects.Insert(pivot);
                }

                _uow.ModuleSubjects.Save();
            }

            return RedirectToAction("Index");
        }


        // GET: Module/Edit/5
        public ActionResult Edit(int id)
        {
            var module = _uow.Modules.GetById(id);
            if (module == null) return HttpNotFound();

            var vm = new ModuleVM
            {
                Id = module.Id,
                Name = module.Name,
                Code = module.Code,

                ModuleSubjects = module.ModuleSubjects
                    .Select(ms => new ModuleSubjectVM
                    {
                        Id = ms.Id,
                        SubjectId = ms.SubjectId,
                        SubjectName = ms.Subject.Name,
                        SubjectCode = ms.Subject.Code
                    })
                    .ToList()
            };

            vm.AvailableSubjects = _uow.Subjects.GetAll()
                .Select(s => new SubjectLookupVM
                {
                    Id = s.Id,
                    Name = $"{s.Code} : {s.Name}"
                })
                .OrderBy(x => x.Name)
                .ToList();

            return View(vm);
        }


        // POST: Module/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModuleVM model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableSubjects = _uow.Subjects.GetAll()
                    .Select(s => new SubjectLookupVM
                    {
                        Id = s.Id,
                        Name = $"{s.Code} : {s.Name}"
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                return View(model);
            }

            var module = _uow.Modules.GetById(model.Id);
            if (module == null) return HttpNotFound();

            // Update module fields
            module.Name = model.Name;
            module.Code = model.Code;
            module.UpdatedAt = DateTime.Now;

            _uow.Modules.Update(module);
            _uow.Modules.Save();

            // Submitted subject IDs
            var newSubjectIds = (model.ModuleSubjects ?? new List<ModuleSubjectVM>())
                .Select(x => x.SubjectId)
                .Distinct()
                .ToList();

            // Existing subject IDs in DB
            var existingPivots = _uow.ModuleSubjects.GetAll()
                .Where(ms => ms.ModuleId == module.Id)
                .ToList();

            var existingSubjectIds = existingPivots.Select(x => x.SubjectId).ToList();

            // 1) Remove deleted subjects
            var toRemove = existingPivots.Where(x => !newSubjectIds.Contains(x.SubjectId)).ToList();
            foreach (var pivot in toRemove)
                _uow.ModuleSubjects.Delete(pivot.Id);

            // 2) Add newly selected subjects
            var toAdd = newSubjectIds.Where(id => !existingSubjectIds.Contains(id)).ToList();
            foreach (var subjectId in toAdd)
            {
                _uow.ModuleSubjects.Insert(new ModuleSubject
                {
                    ModuleId = module.Id,
                    SubjectId = subjectId
                });
            }

            _uow.ModuleSubjects.Save();

            return RedirectToAction("Index");
        }


        // GET: Module/Delete/5
        public ActionResult Delete(int id)
        {
            var module = _uow.Modules.GetById(id);
            if (module == null)
                return HttpNotFound();

            var model = new ModuleVM
            {
                Id = module.Id,
                Name = module.Name,
                Code = module.Code,
            };

            return View(model);
        }

        // POST: Module/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var module = _uow.Modules.GetById(id);
            if (module == null)
                return HttpNotFound();

            // 1) delete pivot rows first
            var pivots = _uow.ModuleSubjects
                .GetAll()
                .Where(ms => ms.ModuleId == id)
                .ToList();

            foreach (var ms in pivots)
                _uow.ModuleSubjects.Delete(ms.Id);

            _uow.ModuleSubjects.Save();

            // 2) delete the module
            _uow.Modules.Delete(module.Id);
            _uow.Modules.Save();

            return RedirectToAction("Index");
        }

    }
}
