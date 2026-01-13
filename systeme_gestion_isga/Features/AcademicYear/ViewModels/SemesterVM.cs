using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Features.AcademicYear.ViewModels
{
    public class SemesterVM
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required]
        public int Order { get; set; }

    }
}