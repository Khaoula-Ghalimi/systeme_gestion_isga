namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMissingThings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ModuleSubjects", "Student_UserID", "dbo.Students");
            DropIndex("dbo.ModuleSubjects", new[] { "Student_UserID" });
            DropIndex("dbo.SemesterModules", new[] { "SemesterId" });
            DropIndex("dbo.SemesterModules", new[] { "ModuleId" });
            DropPrimaryKey("dbo.SemesterModules");
            AddColumn("dbo.ModuleSubjects", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.ModuleSubjects", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.SemesterModules", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.SemesterModules", "Id");
            CreateIndex("dbo.SemesterModules", new[] { "SemesterId", "ModuleId" }, unique: true, name: "IX_Semester_Module");
            DropColumn("dbo.ModuleSubjects", "Student_UserID");
            DropColumn("dbo.SemesterModules", "CreatedAt");
            DropColumn("dbo.SemesterModules", "UpdatedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SemesterModules", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.SemesterModules", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.ModuleSubjects", "Student_UserID", c => c.Int());
            DropIndex("dbo.SemesterModules", "IX_Semester_Module");
            DropPrimaryKey("dbo.SemesterModules");
            DropColumn("dbo.SemesterModules", "Id");
            DropColumn("dbo.ModuleSubjects", "UpdatedAt");
            DropColumn("dbo.ModuleSubjects", "CreatedAt");
            AddPrimaryKey("dbo.SemesterModules", new[] { "SemesterId", "ModuleId" });
            CreateIndex("dbo.SemesterModules", "ModuleId");
            CreateIndex("dbo.SemesterModules", "SemesterId");
            CreateIndex("dbo.ModuleSubjects", "Student_UserID");
            AddForeignKey("dbo.ModuleSubjects", "Student_UserID", "dbo.Students", "UserID");
        }
    }
}
