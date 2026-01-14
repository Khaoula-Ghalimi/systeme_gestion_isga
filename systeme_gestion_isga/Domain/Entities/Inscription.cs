using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using systeme_gestion_isga.Domain.Enums;

namespace systeme_gestion_isga.Domain.Entities
{
    public class Inscription : ModelBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }


        //[Required]
        //public int AcademicYearId { get; set; }
        //[ForeignKey(nameof(AcademicYearId))]
        //public virtual AcademicYear AcademicYear { get; set; }

        ////[Required]
        ////public int ProgramId { get; set; }
        ////[ForeignKey(nameof(ProgramId))]
        ////public virtual Program Program { get; set; }

        //[Required]
        //public int ProgramAcademicYearId { get; set; }
        //[ForeignKey(nameof(ProgramAcademicYearId))]
        //public virtual ProgramAcademicYear ProgramAcademicYear { get; set; }

        [Required]
        public int LevelId { get; set; }
        [ForeignKey(nameof(LevelId))]
        public virtual Level Level { get; set; }

        [Required]
        public DateTime InscriptionDate { get; set; }

        [Required]
        public InscriptionStatus Status { get; set; }

        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    }
}