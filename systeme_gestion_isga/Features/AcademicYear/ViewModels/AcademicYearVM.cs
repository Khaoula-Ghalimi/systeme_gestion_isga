using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace systeme_gestion_isga.Features.AcademicYear.ViewModels
{
    public class AcademicYearVM
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }


        public List<ProgramAcademicYearVM> AssignedPrograms { get; set; } = new List<ProgramAcademicYearVM>();
        public List<SelectListItem> AvailablePrograms { get; set; } = new List<SelectListItem>();

    }
}