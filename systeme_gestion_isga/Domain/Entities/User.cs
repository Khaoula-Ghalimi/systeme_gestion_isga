using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using systeme_gestion_isga.Domain.Enums;

namespace systeme_gestion_isga.Domain.Entities
{
    public class User : ModelBase
    {
        // Unique identifier

        [Key]
        public int Id { get; set; }

        // Authentication information
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public UserRole Role { get; set; }

        // Profile information
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }


        // Navigation properties
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }


    }
}