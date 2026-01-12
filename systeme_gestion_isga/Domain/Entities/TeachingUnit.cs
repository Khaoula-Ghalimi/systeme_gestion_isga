using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Domain.Entities
{
    public class TeachingUnit : ModelBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher Teacher { get; set; }

        [Required]
        public int AcademicYearId { get; set; }
        [ForeignKey(nameof(AcademicYearId))]
        public virtual AcademicYear AcademicYear { get; set; }

        [Required]
        public int SemesterId { get; set; }
        [ForeignKey(nameof(SemesterId))]
        public virtual Semester Semester { get; set; }

        [Required]
        public int ModuleSubjectId { get; set; }
        [ForeignKey(nameof(ModuleSubjectId))]
        public virtual ModuleSubject ModuleSubject { get; set; }
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();



    }
}