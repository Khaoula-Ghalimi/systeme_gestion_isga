using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace systeme_gestion_isga.Features.Module.ViewModels
{
    public class ModuleVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        public List<ModuleSubjectVM> ModuleSubjects { get; set; } = new List<ModuleSubjectVM>();

        public List<SubjectLookupVM> AvailableSubjects { get; set; } = new List<SubjectLookupVM>();


    }
}
