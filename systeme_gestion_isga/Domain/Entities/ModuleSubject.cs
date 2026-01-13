using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Domain.Entities
{
    public class ModuleSubject : ModelBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ModuleId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [ForeignKey(nameof(ModuleId))]
        public virtual Module Module { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; }

        public virtual ICollection<TeachingUnit> TeachingUnits { get; set; } = new List<TeachingUnit>();


    }
}