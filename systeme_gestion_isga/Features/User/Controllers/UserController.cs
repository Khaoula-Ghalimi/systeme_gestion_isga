using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using system_gestion_isga.Infrastructure.Utils;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Domain.Enums;
using systeme_gestion_isga.Features.User.ViewModels;
using systeme_gestion_isga.Features.Users.ViewModels;
// ✅ use UnitOfWork
using systeme_gestion_isga.Infrastructure.UnitOfWork;

namespace systeme_gestion_isga.Features.Users.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _uow;

        public UserController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public UserController()
        {
            _uow = new UnitOfWork();
        }

        // GET: User
        public ActionResult Index()
        {
            var users = _uow.Users.GetAll().OrderBy(u => u.Id).ToList();
            return View(users);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View(new UserCreateVM());
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_uow.Users.Exists(model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            var user = new Domain.Entities.User
            {
                Username = model.Username,
                PasswordHash = PasswordHasher.Hash(model.Password),
                Role = model.Role,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone
            };

            _uow.Users.Insert(user);

            if (model.Role == UserRole.Student)
            {
                var student = new Student
                {
                    UserID = user.Id,
                    StudentNumber = model.Student.StudentNumber,
                    CIN = model.Student.CIN,
                    BirthDate = model.Student.BirthDate,
                    Gender = (Gender)model.Student.Gender,
                    Address = model.Student.Address,
                };

                _uow.Students.Insert(student);
            }
            else if (model.Role == UserRole.Teacher)
            {
                var teacher = new Teacher
                {
                    UserID = user.Id,
                    EmployeeNumber = model.Teacher.EmployeeNumber,
                    CIN = model.Teacher.CIN,
                    BirthDate = model.Teacher.BirthDate,
                    Gender = (Gender)model.Teacher.Gender,
                    Address = model.Teacher.Address,
                };

                _uow.Teachers.Insert(teacher);
            }

            // ✅ ONE SAVE ONLY
            _uow.Save();

            return RedirectToAction("Index");
        }

        // GET: User/Edit/{id}
        public ActionResult Edit(int id)
        {
            var user = _uow.Users.GetById(id);
            if (user == null) return HttpNotFound();

            var model = new UserEditVM
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,

                Student = new StudentVM(),
                Teacher = new TeacherVM()
            };

            if (user.Role == UserRole.Student)
            {
                // IMPORTANT: if Student PK != UserID, use a GetByUserId method instead
                var student = _uow.Students.GetById(user.Id);
                if (student != null)
                {
                    model.Student = new StudentVM
                    {
                        StudentNumber = student.StudentNumber,
                        CIN = student.CIN,
                        BirthDate = student.BirthDate,
                        Gender = student.Gender,
                        Address = student.Address
                    };
                }
            }
            else if (user.Role == UserRole.Teacher)
            {
                var teacher = _uow.Teachers.GetById(user.Id);
                if (teacher != null)
                {
                    model.Teacher = new TeacherVM
                    {
                        EmployeeNumber = teacher.EmployeeNumber,
                        CIN = teacher.CIN,
                        BirthDate = teacher.BirthDate,
                        Gender = teacher.Gender,
                        Address = teacher.Address
                    };
                }
            }

            return View(model);
        }

        // POST: User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _uow.Users.GetById(model.Id);
            if (user == null) return HttpNotFound();

            if (!string.Equals(user.Username, model.Username, StringComparison.OrdinalIgnoreCase)
                && _uow.Users.Exists(model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            var oldRole = user.Role;

            // update user
            user.Username = model.Username;

            if (!string.IsNullOrEmpty(model.NewPassword))
                user.PasswordHash = PasswordHasher.Hash(model.NewPassword);

            user.Role = model.Role;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Phone = model.Phone;

            _uow.Users.Update(user);

            // role changed: remove old child
            if (oldRole != model.Role)
            {
                if (oldRole == UserRole.Student)
                {
                    var oldStudent = _uow.Students.GetById(user.Id);
                    if (oldStudent != null) _uow.Students.Delete(user.Id);
                }
                else if (oldRole == UserRole.Teacher)
                {
                    var oldTeacher = _uow.Teachers.GetById(user.Id);
                    if (oldTeacher != null) _uow.Teachers.Delete(user.Id);
                }
            }

            // upsert new child
            if (model.Role == UserRole.Student)
            {
                var student = _uow.Students.GetById(user.Id);
                if (student == null)
                {
                    student = new Student { UserID = user.Id };
                    _uow.Students.Insert(student);
                }

                student.StudentNumber = model.Student.StudentNumber;
                student.CIN = model.Student.CIN;
                student.BirthDate = model.Student.BirthDate;
                student.Gender = (Gender)model.Student.Gender;
                student.Address = model.Student.Address;
            }
            else if (model.Role == UserRole.Teacher)
            {
                var teacher = _uow.Teachers.GetById(user.Id);
                if (teacher == null)
                {
                    teacher = new Teacher { UserID = user.Id };
                    _uow.Teachers.Insert(teacher);
                }

                teacher.EmployeeNumber = model.Teacher.EmployeeNumber;
                teacher.CIN = model.Teacher.CIN;
                teacher.BirthDate = model.Teacher.BirthDate;
                teacher.Gender = (Gender)model.Teacher.Gender;
                teacher.Address = model.Teacher.Address;
            }

            // ✅ ONE SAVE ONLY
            _uow.Save();

            return RedirectToAction("Index");
        }

        // GET: User/Delete/{id}
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = _uow.Users.GetById(id.Value);
            if (user == null) return HttpNotFound();

            return View(user);
        }

        // POST: User/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = _uow.Users.GetById(id);
            if (user == null) return HttpNotFound();

            // if you DON'T have cascade delete, delete child first:
            // if (user.Role == UserRole.Student) _uow.Students.Delete(id);
            // else if (user.Role == UserRole.Teacher) _uow.Teachers.Delete(id);

            _uow.Users.Delete(id);

            // ✅ ONE SAVE ONLY
            _uow.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _uow.Dispose();
            base.Dispose(disposing);
        }
    }
}
