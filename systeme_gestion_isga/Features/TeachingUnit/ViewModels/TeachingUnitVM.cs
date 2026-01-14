using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace systeme_gestion_isga.Features.TeachingUnit.ViewModels
{
    public class TeachingUnitVM
    {
        public int Id { get; set; }

        // ====== Selected IDs (what you save) ======
        [Required]
        [Display(Name = "Academic Year")]
        public int AcademicYearId { get; set; }

        [Required]
        [Display(Name = "Level")]
        public int LevelId { get; set; }

        [Required]
        [Display(Name = "Semester")]
        public int SemesterId { get; set; }

        [Required]
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }

        [Required]
        [Display(Name = "Module / Matiere")]
        public int ModuleMatiereId { get; set; } // keep same naming as your entity

        // ====== Optional display fields (nice for Index/Details) ======
        public string AcademicYearName { get; set; }
        public string LevelName { get; set; }
        public string SemesterName { get; set; }
        public string TeacherName { get; set; }
        public string ModuleMatiereName { get; set; }

        // ====== Available lists (for dropdowns) ======
        public List<SelectListItem> AcademicYears { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Levels { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Semesters { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Teachers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ModuleMatieres { get; set; } = new List<SelectListItem>();

    }
}
