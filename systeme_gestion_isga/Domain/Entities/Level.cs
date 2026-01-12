using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Domain.Entities
{
    public class Level : ModelBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        public int Order { get; set; }

        //[Required]
        //public int ProgramId { get; set; }

        //[ForeignKey(nameof(ProgramId))]
        //public Program Program { get; set; }

        [Required]
        public int ProgramAcademicYearId { get; set; }
        [ForeignKey(nameof(ProgramAcademicYearId))]
        public virtual ProgramAcademicYear ProgramAcademicYear { get; set; }


        public virtual ICollection<Semester> Semesters { get; set; } = new List<Semester>();
        public virtual ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();
    }
}