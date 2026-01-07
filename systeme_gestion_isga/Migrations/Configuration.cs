namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using systeme_gestion_isga.Infrastructure.Seeders.Database;

    internal sealed class Configuration : DbMigrationsConfiguration<systeme_gestion_isga.Infrastructure.Database.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(systeme_gestion_isga.Infrastructure.Database.AppDbContext context)
        {
            UserSeeder.SeedAdmin(context);
        }
    }
}
