using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Features.Program.ViewModels
{
    public class LevelVM
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        [Required]
        public int Order { get; set; }

        public List<SemesterVM> Semesters { get; set; } = new List<SemesterVM>();

    }
}