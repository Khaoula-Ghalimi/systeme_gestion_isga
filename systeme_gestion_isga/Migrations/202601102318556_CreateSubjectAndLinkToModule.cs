namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateSubjectAndLinkToModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModuleSubjects",
                c => new
                    {
                        ModuleId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ModuleId, t.SubjectId })
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.ModuleId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(maxLength: 10),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SemesterModules", "Subject_Id", c => c.Int());
            CreateIndex("dbo.SemesterModules", "Subject_Id");
            AddForeignKey("dbo.SemesterModules", "Subject_Id", "dbo.Subjects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModuleSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.SemesterModules", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.ModuleSubjects", "ModuleId", "dbo.Modules");
            DropIndex("dbo.ModuleSubjects", new[] { "SubjectId" });
            DropIndex("dbo.ModuleSubjects", new[] { "ModuleId" });
            DropIndex("dbo.SemesterModules", new[] { "Subject_Id" });
            DropColumn("dbo.SemesterModules", "Subject_Id");
            DropTable("dbo.Subjects");
            DropTable("dbo.ModuleSubjects");
        }
    }
}
