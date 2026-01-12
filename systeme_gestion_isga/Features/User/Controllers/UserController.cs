using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using system_gestion_isga.Infrastructure.Repositories.Users;
using system_gestion_isga.Infrastructure.Utils;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Domain.Enums;
using systeme_gestion_isga.Features.User.ViewModels;
using systeme_gestion_isga.Features.Users.ViewModels;
using systeme_gestion_isga.Infrastructure.Repositories.Students;
using systeme_gestion_isga.Infrastructure.Repositories.Teachers;



namespace systeme_gestion_isga.Features.Users.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _users;
        private readonly ITeacherRepository _teachers;
        private readonly IStudentRepository _students;
        public UserController(IUserRepository users, ITeacherRepository teacher, IStudentRepository student)
        {
            _users = users;
            _teachers = teacher;
            _students = student;
        }
        public UserController()
        {
            _users = new UserRepository();
            _teachers = new TeacherRepository();
            _students = new StudentRepository();
        }
        // GET: User
        public ActionResult Index()
        {
            var users = _users.GetAll().OrderBy(u => u.Id).ToList();
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
            if (ModelState.IsValid)
            {
                if (_users.Exists(model.Username))
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
                _users.Insert(user);
                _users.Save();

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
                    _students.Insert(student);
                    _students.Save();
                } else if (model.Role == UserRole.Teacher)
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
                    _teachers.Insert(teacher);
                    _teachers.Save();
                }
                
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: User/Edit/{id}
        public ActionResult Edit(int id)
        {
            var user = _users.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
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
                var student = _students.GetById(user.Id);
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
                var teacher = _teachers.GetById(user.Id);
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

            var user = _users.GetById(model.Id);
            if (user == null) return HttpNotFound();

            // username uniqueness
            if (!string.Equals(user.Username, model.Username, StringComparison.OrdinalIgnoreCase)
                && _users.Exists(model.Username))
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

            _users.Update(user);
            _users.Save();

            // if role changed: remove old child row
            if (oldRole != model.Role)
            {
                if (oldRole == UserRole.Student)
                {
                    var oldStudent = _students.GetById(user.Id);
                    if (oldStudent != null)
                    {
                        _students.Delete(user.Id);
                        _students.Save();
                    }
                }
                else if (oldRole == UserRole.Teacher)
                {
                    var oldTeacher = _teachers.GetById(user.Id);
                    if (oldTeacher != null)
                    {
                        _teachers.Delete(user.Id);
                        _teachers.Save();
                    }
                }
            }

            // upsert new child row
            if (model.Role == UserRole.Student)
            {
                var student = _students.GetById(user.Id);
                if (student == null)
                {
                    student = new Student { UserID = user.Id };
                    _students.Insert(student);
                }

                student.StudentNumber = model.Student.StudentNumber;
                student.CIN = model.Student.CIN;
                student.BirthDate = model.Student.BirthDate;
                student.Gender = (Gender)model.Student.Gender;
                student.Address = model.Student.Address;

                _students.Save();
            }
            else if (model.Role == UserRole.Teacher)
            {
                var teacher = _teachers.GetById(user.Id);
                if (teacher == null)
                {
                    teacher = new Teacher { UserID = user.Id };
                    _teachers.Insert(teacher);
                }

                teacher.EmployeeNumber = model.Teacher.EmployeeNumber;
                teacher.CIN = model.Teacher.CIN;
                teacher.BirthDate = model.Teacher.BirthDate;
                teacher.Gender = (Gender)model.Teacher.Gender;
                teacher.Address = model.Teacher.Address;

                _teachers.Save();
            }

            return RedirectToAction("Index");
        }


        // GET: User/Delete/{id}
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = _users.GetById(id.Value);
            if (user == null) return HttpNotFound();

            return View(user);
        }

        // POST: User/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = _users.GetById(id);
            if (user == null) return HttpNotFound();

            // delete child first
            //if (user.Role == UserRole.Student)
            //{
            //    var student = _students.GetById(id);
            //    if (student != null)
            //    {
            //        _students.Delete(id);
            //        _students.Save();
            //    }
            //}
            //else if (user.Role == UserRole.Teacher)
            //{
            //    var teacher = _teachers.GetById(id);
            //    if (teacher != null)
            //    {
            //        _teachers.Delete(id);
            //        _teachers.Save();
            //    }
            //}

            // then delete user
            _users.Delete(id);
            _users.Save();

            return RedirectToAction("Index");
        }

    }
}