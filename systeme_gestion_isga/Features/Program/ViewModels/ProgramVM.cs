using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Features.Program.ViewModels
{
    public class ProgramVM
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        public int DurationInYears { get; set; }
        //public List<LevelVM> Levels { get; set; } = new List<LevelVM>();

    }
}