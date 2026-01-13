using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace systeme_gestion_isga.Features.Semester.ViewModels
{
    public class SemesterEditModulesVM
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int SemesterOrder { get; set; }

        public List<SemesterModuleVM> SemesterModules { get; set; } = new List<SemesterModuleVM>();
        public List<ModuleLookupVM> AvailableModules { get; set; } = new List<ModuleLookupVM>();
    }

    public class SemesterModuleVM
    {
        public int Id { get; set; }          // pivot id (optional)
        [Required]
        public int ModuleId { get; set; }

        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
    }

    public class ModuleLookupVM
    {
        public int Id { get; set; }
        public string Name { get; set; } // "CODE : Name"
    }
}
