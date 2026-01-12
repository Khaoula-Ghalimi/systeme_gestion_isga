namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ModuleSubjects", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.SemesterModules", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.Semesters", "LevelId", "dbo.Levels");
            DropForeignKey("dbo.Levels", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Teachers", "UserID", "dbo.Users");
            DropForeignKey("dbo.Students", "UserID", "dbo.Users");
            AddForeignKey("dbo.ModuleSubjects", "ModuleId", "dbo.Modules", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SemesterModules", "SemesterId", "dbo.Semesters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Semesters", "LevelId", "dbo.Levels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Levels", "ProgramId", "dbo.Programs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Teachers", "UserID", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Students", "UserID", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "UserID", "dbo.Users");
            DropForeignKey("dbo.Teachers", "UserID", "dbo.Users");
            DropForeignKey("dbo.Levels", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Semesters", "LevelId", "dbo.Levels");
            DropForeignKey("dbo.SemesterModules", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.ModuleSubjects", "ModuleId", "dbo.Modules");
            AddForeignKey("dbo.Students", "UserID", "dbo.Users", "Id");
            AddForeignKey("dbo.Teachers", "UserID", "dbo.Users", "Id");
            AddForeignKey("dbo.Levels", "ProgramId", "dbo.Programs", "Id");
            AddForeignKey("dbo.Semesters", "LevelId", "dbo.Levels", "Id");
            AddForeignKey("dbo.SemesterModules", "SemesterId", "dbo.Semesters", "Id");
            AddForeignKey("dbo.ModuleSubjects", "ModuleId", "dbo.Modules", "Id");
        }
    }
}
