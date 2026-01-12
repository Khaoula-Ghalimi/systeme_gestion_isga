using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Domain.Entities
{
    public class Grade : ModelBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int InscriptionId { get; set; }

        [ForeignKey(nameof(InscriptionId))]
        public virtual Inscription Inscription { get; set; }

        [Required]
        public int TeachingUnitId { get; set; }

        [ForeignKey(nameof(TeachingUnitId))]
        public virtual TeachingUnit TeachingUnit { get; set; }

        // Marks
        public decimal? Exam1 { get; set; }
        public decimal? Exam2 { get; set; }
        public decimal? ExamRat { get; set; }
    }
}