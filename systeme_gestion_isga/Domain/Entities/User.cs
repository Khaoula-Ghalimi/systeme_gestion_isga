using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using systeme_gestion_isga.Domain.Enums;

namespace systeme_gestion_isga.Domain.Entities
{
    public class User : ModelBase
    {
        // Unique identifier
        public int Id { get; set; }

        // Authentication information
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }

        // Profile information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }


        // Navigation properties
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }


    }
}