using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace systeme_gestion_isga.Domain.Entities
{
    public class Teacher : ModelBase
    {
        [Key, ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }


    }
}