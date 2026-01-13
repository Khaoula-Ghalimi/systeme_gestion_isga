using System;
using System.Linq;
using System.Web.Mvc;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Features.Subject.ViewModels;
using systeme_gestion_isga.Infrastructure.UnitOfWork;

namespace systeme_gestion_isga.Features.Subject.Controllers
{
    public class SubjectController : Controller
    {
        private readonly IUnitOfWork _uow;

        public SubjectController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public SubjectController()
        {
            _uow = new UnitOfWork();
        }

        // GET: Subject
        public ActionResult Index()
        {
            var subjects = _uow.Subjects
                .GetAll()
                .Select(s => new SubjectVM
                {
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Code
                })
                .OrderBy(s => s.Id)
                .ToList();

            return View(subjects);
        }

        // GET: Subject/Create
        public ActionResult Create()
        {
            return View(new SubjectVM());
        }

        // POST: Subject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubjectVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var subject = new systeme_gestion_isga.Domain.Entities.Subject
            {
                Name = model.Name,
                Code = model.Code,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _uow.Subjects.Insert(subject);
            _uow.Subjects.Save();

            return RedirectToAction("Index");
        }

        // GET: Subject/Edit/5
        public ActionResult Edit(int id)
        {
            var subject = _uow.Subjects.GetById(id);
            if (subject == null)
                return HttpNotFound();

            var model = new SubjectVM
            {
                Id = subject.Id,
                Name = subject.Name,
                Code = subject.Code
            };

            return View(model);
        }

        // POST: Subject/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubjectVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var subject = _uow.Subjects.GetById(model.Id);
            if (subject == null)
                return HttpNotFound();

            subject.Name = model.Name;
            subject.Code = model.Code;
            subject.UpdatedAt = DateTime.Now;

            _uow.Subjects.Update(subject);
            _uow.Subjects.Save();

            return RedirectToAction("Index");
        }

        // GET: Subject/Delete/5
        public ActionResult Delete(int id)
        {
            var subject = _uow.Subjects.GetById(id);
            if (subject == null)
                return HttpNotFound();

            var model = new SubjectVM
            {
                Id = subject.Id,
                Name = subject.Name,
                Code = subject.Code
            };

            return View(model);
        }

        // POST: Subject/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var subject = _uow.Subjects.GetById(id);
            if (subject == null)
                return HttpNotFound();

            // If Subject is linked to ModuleSubjects, deletion may fail depending on cascade rules.
            // If needed, you can delete links first.
            // foreach (var ms in subject.ModuleSubjects.ToList()) _uow.ModuleSubjects.Delete(ms.Id);

            _uow.Subjects.Delete(subject.Id);
            _uow.Subjects.Save();

            return RedirectToAction("Index");
        }
    }
}
