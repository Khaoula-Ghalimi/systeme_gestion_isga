using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using systeme_gestion_isga.Domain.Entities;

namespace systeme_gestion_isga.Features.Module.ViewModels
{
    public class ModuleSubjectVM
    {
        public int Id { get; set; }

        [Required]
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }

        // Optional: show teaching units under this subject
        //public List<TeachingUnitVM> TeachingUnits { get; set; } = new List<TeachingUnitVM>();
    }
}
