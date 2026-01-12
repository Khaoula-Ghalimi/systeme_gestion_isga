namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateModuleAndLinkToSemester : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SemesterModules",
                c => new
                    {
                        SemesterId = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.SemesterId, t.ModuleId })
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.Semesters", t => t.SemesterId, cascadeDelete: true)
                .Index(t => t.SemesterId)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(maxLength: 10),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SemesterModules", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.SemesterModules", "ModuleId", "dbo.Modules");
            DropIndex("dbo.SemesterModules", new[] { "ModuleId" });
            DropIndex("dbo.SemesterModules", new[] { "SemesterId" });
            DropTable("dbo.Modules");
            DropTable("dbo.SemesterModules");
        }
    }
}
