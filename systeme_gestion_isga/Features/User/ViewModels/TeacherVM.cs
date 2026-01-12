using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using systeme_gestion_isga.Domain.Enums;

namespace systeme_gestion_isga.Features.User.ViewModels
{
    public class TeacherVM
    {
        public string EmployeeNumber { get; set; }
        public string CIN { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public Gender? Gender { get; set; }
    }
}