using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace systeme_gestion_isga.Features.Inscription.ViewModels
{
    public class InscriptionVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Academic Year")]
        public int? AcademicYearId { get; set; }

        [Required]
        [Display(Name = "Program")]
        public int? ProgramAcademicYearId { get; set; }

        [Required]
        [Display(Name = "Level")]
        public int? LevelId { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int? StudentId { get; set; }

        public String StudentName { get; set; }

        [Required]
        public DateTime InscriptionDate { get; set; } = DateTime.UtcNow;


        // Dropdowns
        public IEnumerable<SelectListItem> AcademicYears { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ProgramAcademicYears { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Levels { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Students { get; set; } = new List<SelectListItem>();
    }
}
