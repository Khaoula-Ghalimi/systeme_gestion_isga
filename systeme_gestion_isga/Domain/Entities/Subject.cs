using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Domain.Entities
{
    public class Subject : ModelBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(10)]
        public string Code { get; set; }

        public virtual ICollection<ModuleSubject> ModuleSubjects { get; set; } = new List<ModuleSubject>();

    }
}