using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Domain.Entities
{
    public class SemesterModule : ModelBase
    {
        [Key, Column(Order = 0)]
        public int SemesterId { get; set; }

        [Key, Column(Order = 1)]
        public int ModuleId { get; set; }

        [ForeignKey(nameof(SemesterId))]
        public virtual Semester Semester { get; set; }

        [ForeignKey(nameof(ModuleId))]
        public virtual Module Module { get; set; }

    }
}