namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInscription : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SemesterModules", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.Levels", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Semesters", "LevelId", "dbo.Levels");
            DropForeignKey("dbo.SemesterModules", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.SemesterModules", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.ModuleSubjects", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.ModuleSubjects", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.SemesterModules", new[] { "Subject_Id" });
            CreateTable(
                "dbo.AcademicYears",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        AcademicYearId = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        LevelId = c.Int(nullable: false),
                        InscriptionDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYears", t => t.AcademicYearId)
                .ForeignKey("dbo.Levels", t => t.LevelId)
                .ForeignKey("dbo.Programs", t => t.ProgramId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.AcademicYearId)
                .Index(t => t.ProgramId)
                .Index(t => t.LevelId);
            
            AddColumn("dbo.ModuleSubjects", "Student_UserID", c => c.Int());
            CreateIndex("dbo.ModuleSubjects", "Student_UserID");
            AddForeignKey("dbo.ModuleSubjects", "Student_UserID", "dbo.Students", "UserID");
            AddForeignKey("dbo.Levels", "ProgramId", "dbo.Programs", "Id");
            AddForeignKey("dbo.Semesters", "LevelId", "dbo.Levels", "Id");
            AddForeignKey("dbo.SemesterModules", "SemesterId", "dbo.Semesters", "Id");
            AddForeignKey("dbo.SemesterModules", "ModuleId", "dbo.Modules", "Id");
            AddForeignKey("dbo.ModuleSubjects", "ModuleId", "dbo.Modules", "Id");
            AddForeignKey("dbo.ModuleSubjects", "SubjectId", "dbo.Subjects", "Id");
            DropColumn("dbo.SemesterModules", "Subject_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SemesterModules", "Subject_Id", c => c.Int());
            DropForeignKey("dbo.ModuleSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ModuleSubjects", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.SemesterModules", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.SemesterModules", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.Semesters", "LevelId", "dbo.Levels");
            DropForeignKey("dbo.Levels", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Inscriptions", "StudentId", "dbo.Students");
            DropForeignKey("dbo.ModuleSubjects", "Student_UserID", "dbo.Students");
            DropForeignKey("dbo.Inscriptions", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Inscriptions", "LevelId", "dbo.Levels");
            DropForeignKey("dbo.Inscriptions", "AcademicYearId", "dbo.AcademicYears");
            DropIndex("dbo.ModuleSubjects", new[] { "Student_UserID" });
            DropIndex("dbo.Inscriptions", new[] { "LevelId" });
            DropIndex("dbo.Inscriptions", new[] { "ProgramId" });
            DropIndex("dbo.Inscriptions", new[] { "AcademicYearId" });
            DropIndex("dbo.Inscriptions", new[] { "StudentId" });
            DropColumn("dbo.ModuleSubjects", "Student_UserID");
            DropTable("dbo.Inscriptions");
            DropTable("dbo.AcademicYears");
            CreateIndex("dbo.SemesterModules", "Subject_Id");
            AddForeignKey("dbo.ModuleSubjects", "SubjectId", "dbo.Subjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ModuleSubjects", "ModuleId", "dbo.Modules", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SemesterModules", "ModuleId", "dbo.Modules", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SemesterModules", "SemesterId", "dbo.Semesters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Semesters", "LevelId", "dbo.Levels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Levels", "ProgramId", "dbo.Programs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SemesterModules", "Subject_Id", "dbo.Subjects", "Id");
        }
    }
}
