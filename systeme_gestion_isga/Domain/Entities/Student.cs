using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using systeme_gestion_isga.Domain.Enums;

namespace systeme_gestion_isga.Domain.Entities
{
    public class Student : ModelBase
    {
        [Key, ForeignKey("User")]
        public int UserID { get; set; }


        [Required]
        [StringLength(50)]
        public string StudentNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string CIN { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }


        public virtual User User { get; set; }

        public virtual ICollection<ModuleSubject> ModuleSubjects { get; set; } = new List<ModuleSubject>();

        public virtual ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();


    }
}