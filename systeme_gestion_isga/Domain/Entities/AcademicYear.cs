using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Domain.Entities
{
    public class AcademicYear : ModelBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();

        public virtual ICollection<TeachingUnit> TeachingUnits { get; set; } = new List<TeachingUnit>();

        public virtual ICollection<ProgramAcademicYear> ProgramAcademicYears { get; set; }



    }
}