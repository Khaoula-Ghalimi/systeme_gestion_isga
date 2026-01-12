using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace systeme_gestion_isga.Domain.Entities
{
    public class ProgramAcademicYear : ModelBase
    {
        [Key]
        public int Id { get; set; }

        // FK -> Program
        [Required, Index("IX_Program_AcYear", 1, IsUnique = true)]
        public int ProgramId { get; set; }

        // FK -> AcademicYear
        [Required, Index("IX_Program_AcYear", 2, IsUnique = true)]
        public int AcademicYearId { get; set; }

        // Optional overrides (if program changes name/code/duration in that year)
        [StringLength(100)]
        public string DisplayName { get; set; }

        [StringLength(10)]
        public string DisplayCode { get; set; }

        public int? DurationInYearsOverride { get; set; }

        public bool IsActive { get; set; } = true;

        // Nav props
        [ForeignKey(nameof(ProgramId))]
        public virtual Program Program { get; set; }

        [ForeignKey(nameof(AcademicYearId))]
        public virtual AcademicYear AcademicYear { get; set; }

        // If you want levels to be versioned by year (recommended)
        public virtual ICollection<Level> Levels { get; set; }

        // If you want inscriptions to target the version (recommended)
        public virtual ICollection<Inscription> Inscriptions { get; set; }
    }
}
