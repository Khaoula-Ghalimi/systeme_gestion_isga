//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//using systeme_gestion_isga.Features.Program.ViewModels;
//using systeme_gestion_isga.Infrastructure.Repositories.Levels;
//using systeme_gestion_isga.Infrastructure.Repositories.Programs;
//using systeme_gestion_isga.Infrastructure.Repositories.Semesters;
//using systeme_gestion_isga.Domain.Entities;

//namespace systeme_gestion_isga.Features.Program.Controllers
//{
//    public class ProgramController : Controller
//    {
//        private readonly IProgramRepository _programs;
//        private readonly ILevelRepository _levels;
//        private readonly ISemesterRepository _semester;

//        public ProgramController(IProgramRepository programs, ILevelRepository levels, ISemesterRepository semester)
//        {
//            _programs = programs;
//            _levels = levels;
//            _semester = semester;
//        }

//        public ProgramController()
//        {
//            _programs = new ProgramRepository();
//            _levels = new LevelRepository();
//            _semester = new SemesterRepository();
//        }



//        // GET: Program
//        public ActionResult Index()
//        {
//            var programs = _programs.
//                GetAll()
//                .Select(p => new ProgramVM
//                    {
//                        Id = p.Id,
//                        Name = p.Name,
//                        Code = p.Code,
//                        Description = p.Description,
//                        DurationInYears = p.DurationInYears,
//                    })
//                .OrderBy(p => p.Id)
//                .ToList();
//            return View(programs);
//        }

//        //GET : Program/Create
//        public ActionResult Create()
//        {
//            return View(new ProgramVM());
//        }

//        //POST : Program/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(ProgramVM model)
//        {
//            model.Levels = model.Levels ?? new List<LevelVM>();

//            for(int i=0; i< model.Levels.Count; i++)
//            {
//                if (string.IsNullOrWhiteSpace(model.Levels[i].Name))
//                    ModelState.AddModelError($"Levels[{i}].Name", "Level name is required.");

//                if (model.Levels[i].Order != i + 1)
//                    model.Levels[i].Order = i + 1;
//            }

//            if (!ModelState.IsValid)
//                return View(model);

//            var program = new Domain.Entities.Program
//            {
//                Name = model.Name,
//                Code = model.Code,
//                Description = model.Description,
//                DurationInYears = model.DurationInYears,
//                CreatedAt = DateTime.Now,
//                UpdatedAt = DateTime.Now
//            };
//            _programs.Insert(program);
//            _programs.Save();
            
//            foreach(var lvl in model.Levels)
//            {
//                var level = new Level
//                {
//                    Name = lvl.Name,
//                    Code = lvl.Code,
//                    Order = lvl.Order,
//                    ProgramId = program.Id,
//                    CreatedAt = DateTime.Now,
//                    UpdatedAt = DateTime.Now
//                };
//                _levels.Insert(level);
//            }
//            _levels.Save();

//            var createdLevels = _levels.GetByProgramId(program.Id);

//            foreach (var level in createdLevels)
//            {
//                var s1 = new Semester
//                {
//                    Name = "S1",
//                    Order = 1,
//                    LevelId = level.Id,
//                    CreatedAt = DateTime.Now,
//                    UpdatedAt = DateTime.Now
//                };

//                var s2 = new Semester
//                {
//                    Name = "S2",
//                    Order = 2,
//                    LevelId = level.Id,
//                    CreatedAt = DateTime.Now,
//                    UpdatedAt = DateTime.Now
//                };
//                _semester.Insert(s1);
//                _semester.Insert(s2);
//            }
//            _semester.Save();
//            return RedirectToAction("Index");

//        }
//        // GET : Program/Edit/5
//        public ActionResult Edit(int id)
//        {
//            var program = _programs.GetById(id);
//            if (program == null)
//                return HttpNotFound();
//            var levels = _levels.GetByProgramId(id).OrderBy(l => l.Order).ToList();
//            var model = new ProgramVM
//            {
//                Id = id,
//                Name = program.Name,
//                Code = program.Code,
//                Description = program.Description,
//                DurationInYears = program.DurationInYears,
//                Levels = levels.Select(l => new LevelVM
//                {
//                    Name = l.Name,
//                    Code = l.Code,
//                    Order = l.Order
//                }).ToList()
//            };
//            return View(model);
//        }
//        //POST : Program/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(ProgramVM model)
//        {
//            model.Levels = model.Levels ?? new List<LevelVM>();

//            for (int i = 0; i < model.Levels.Count; i++)
//            {
//                if (string.IsNullOrWhiteSpace(model.Levels[i].Name))
//                    ModelState.AddModelError($"Levels[{i}].Name", "Level name is required.");

//                // enforce correct order
//                model.Levels[i].Order = i + 1;
//            }
//            if (!ModelState.IsValid)
//                return View(model);
//            var program = _programs.GetById(model.Id);
//            if (program == null) return HttpNotFound();
//            program.Name = model.Name;
//            program.Code = model.Code;
//            program.Description = model.Description;
//            program.DurationInYears = model.DurationInYears;
//            _programs.Update(program);
//            _programs.Save();

//            var oldLevels = _levels.GetByProgramId(program.Id).ToList();

//            foreach (var lvl in oldLevels)
//            {
//                // delete semesters first (FK)
//                var sems = _semester.GetByLevelId(lvl.Id).ToList(); 
//                foreach (var s in sems)
//                    _semester.Delete(s.Id);

//                // then delete the level
//                _levels.Delete(lvl.Id);
//            }

//            _semester.Save();
//            _levels.Save();

//            foreach (var lvlVm in model.Levels)
//            {
//                var level = new Level
//                {
//                    Name = lvlVm.Name,
//                    Code = lvlVm.Code,
//                    Order = lvlVm.Order,
//                    ProgramId = program.Id,
//                    CreatedAt = DateTime.Now,
//                    UpdatedAt = DateTime.Now
//                };

//                _levels.Insert(level);
//                _levels.Save(); 

//                var s1 = new Semester
//                {
//                    Name = "S1",
//                    Order = 1,
//                    LevelId = level.Id,
//                    CreatedAt = DateTime.Now,
//                    UpdatedAt = DateTime.Now
//                };

//                var s2 = new Semester
//                {
//                    Name = "S2",
//                    Order = 2,
//                    LevelId = level.Id,
//                    CreatedAt = DateTime.Now,
//                    UpdatedAt = DateTime.Now
//                };

//                _semester.Insert(s1);
//                _semester.Insert(s2);
//                _semester.Save();
//            }

//            return RedirectToAction("Index");
//        }
//        //GET : Program/Delete/5
//        public ActionResult Delete(int id)
//        {
//            var program = _programs.GetById(id);
//            if (program == null)
//                return HttpNotFound();
//            var model = new ProgramVM
//            {
//                Id = program.Id,
//                Name = program.Name,
//                Code = program.Code,
//                Description = program.Description,
//                DurationInYears = program.DurationInYears
//            };
//            return View(model);
//        }

//        //POST : Program/Delete/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            var program = _programs.GetById(id);
//            if (program == null)
//                return HttpNotFound();
//            //var levels = _levels.GetByProgramId(program.Id).ToList();
//            //foreach (var lvl in levels)
//            //{
//            //    var semesters = _semester.GetByLevelId(lvl.Id).ToList();
//            //    foreach (var sem in semesters)
//            //    {
//            //        _semester.Delete(sem.Id);
//            //    }
//            //    _semester.Save();
//            //    _levels.Delete(lvl.Id);
//            //}
//            //_levels.Save();
//            _programs.Delete(program.Id);
//            _programs.Save();
//            return RedirectToAction("Index");
//        }
//    }
//}