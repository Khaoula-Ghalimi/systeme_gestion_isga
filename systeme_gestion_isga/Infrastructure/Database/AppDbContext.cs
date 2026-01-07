using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using systeme_gestion_isga.Domain.Entities;

namespace systeme_gestion_isga.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=SystemGestionISGA")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

    }
}