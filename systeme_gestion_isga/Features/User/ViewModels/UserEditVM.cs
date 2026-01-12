using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using systeme_gestion_isga.Domain.Enums;
using systeme_gestion_isga.Features.User.ViewModels;

namespace systeme_gestion_isga.Features.Users.ViewModels
{
    public class UserEditVM
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        // Optional: leave empty if you don't want to change password
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [StringLength(255), EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }


        public StudentVM Student { get; set; } = new StudentVM() { BirthDate = DateTime.Today };
        public TeacherVM Teacher { get; set; } = new TeacherVM() { BirthDate = DateTime.Today };

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Role == UserRole.Student)
            {
                if (string.IsNullOrWhiteSpace(Student.StudentNumber))
                    yield return new ValidationResult("Student number required", new[] { "Student.StudentNumber" });
                if (string.IsNullOrWhiteSpace(Student.CIN))
                    yield return new ValidationResult("CIN required", new[] { "Student.CIN" });
                if (Student.BirthDate == null)
                    yield return new ValidationResult("Birth date required", new[] { "Student.BirthDate" });
                if (string.IsNullOrWhiteSpace(Student.Address))
                    yield return new ValidationResult("Address required", new[] { "Student.Address" });
                if (Student.Gender == null)
                    yield return new ValidationResult("Gender required", new[] { "Student.Gender" });
            }
            else if (Role == UserRole.Teacher)
            {
                if (string.IsNullOrWhiteSpace(Teacher.EmployeeNumber))
                    yield return new ValidationResult("Employee number required", new[] { "Teacher.EmployeeNumber" });
                if (string.IsNullOrWhiteSpace(Teacher.CIN))
                    yield return new ValidationResult("CIN required", new[] { "Teacher.CIN" });
                if (Teacher.BirthDate == null)
                    yield return new ValidationResult("Birth date required", new[] { "Teacher.BirthDate" });
                if (string.IsNullOrWhiteSpace(Teacher.Address))
                    yield return new ValidationResult("Address required", new[] { "Teacher.Address" });
                if (Teacher.Gender == null)
                    yield return new ValidationResult("Gender required", new[] { "Teacher.Gender" });
            }
        }
    }
}