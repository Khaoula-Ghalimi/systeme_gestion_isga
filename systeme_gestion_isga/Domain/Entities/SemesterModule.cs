using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace systeme_gestion_isga.Domain.Entities
{
    public class SemesterModule
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_Semester_Module", 1, IsUnique = true)]
        public int SemesterId { get; set; }

        [Index("IX_Semester_Module", 2, IsUnique = true)]
        public int ModuleId { get; set; }

        [ForeignKey(nameof(SemesterId))]
        public virtual Semester Semester { get; set; }

        [ForeignKey(nameof(ModuleId))]
        public virtual Module Module { get; set; }

        // extra fields if you want later (Coefficient, IsActive, etc.)
        // public bool IsActive { get; set; } = true;
    }
}
