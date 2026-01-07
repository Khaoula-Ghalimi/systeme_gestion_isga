using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using system_gestion_isga.Infrastructure.Utils;
using systeme_gestion_isga.Domain.Entities;
using systeme_gestion_isga.Domain.Enums;
using systeme_gestion_isga.Infrastructure.Database;

namespace systeme_gestion_isga.Infrastructure.Seeders.Database
{
    public class UserSeeder
    {
        public static void SeedAdmin(AppDbContext context)
        {
            var now = DateTime.Now;
            context.Users.AddOrUpdate(u => u.Username, new User
            {
                Username = "admin",
                PasswordHash = PasswordHasher.Hash("admin123"),
                Role = UserRole.Admin,
                FirstName = "System",
                LastName = "Administrator",
                Email = "admin@isga.ma",
                Phone = "0600000000",
                CreatedAt = now,
                UpdatedAt = now

            });
        }
    }
}