using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Domain.Entities
{
    public class Semester : ModelBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public int LevelId { get; set; }
        [ForeignKey(nameof(LevelId))]
        public Level Level { get; set; }

        public virtual ICollection<SemesterModule> SemesterModules { get; set; } = new List<SemesterModule>();
        public virtual ICollection<TeachingUnit> TeachingUnits { get; set; } = new List<TeachingUnit>();

    }
}