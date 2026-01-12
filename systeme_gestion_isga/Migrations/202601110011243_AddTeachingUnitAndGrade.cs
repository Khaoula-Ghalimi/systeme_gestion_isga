namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeachingUnitAndGrade : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ModuleSubjects");
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InscriptionId = c.Int(nullable: false),
                        TeachingUnitId = c.Int(nullable: false),
                        Exam1 = c.Decimal(precision: 18, scale: 2),
                        Exam2 = c.Decimal(precision: 18, scale: 2),
                        ExamRat = c.Decimal(precision: 18, scale: 2),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inscriptions", t => t.InscriptionId)
                .ForeignKey("dbo.TeachingUnits", t => t.TeachingUnitId)
                .Index(t => t.InscriptionId)
                .Index(t => t.TeachingUnitId);
            
            CreateTable(
                "dbo.TeachingUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeacherId = c.Int(nullable: false),
                        AcademicYearId = c.Int(nullable: false),
                        SemesterId = c.Int(nullable: false),
                        ModuleSubjectId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYears", t => t.AcademicYearId)
                .ForeignKey("dbo.ModuleSubjects", t => t.ModuleSubjectId)
                .ForeignKey("dbo.Semesters", t => t.SemesterId)
                .ForeignKey("dbo.Teachers", t => t.TeacherId)
                .Index(t => t.TeacherId)
                .Index(t => t.AcademicYearId)
                .Index(t => t.SemesterId)
                .Index(t => t.ModuleSubjectId);
            
            AddColumn("dbo.Inscriptions", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Inscriptions", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.ModuleSubjects", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ModuleSubjects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Grades", "TeachingUnitId", "dbo.TeachingUnits");
            DropForeignKey("dbo.TeachingUnits", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.TeachingUnits", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.TeachingUnits", "ModuleSubjectId", "dbo.ModuleSubjects");
            DropForeignKey("dbo.TeachingUnits", "AcademicYearId", "dbo.AcademicYears");
            DropForeignKey("dbo.Grades", "InscriptionId", "dbo.Inscriptions");
            DropIndex("dbo.TeachingUnits", new[] { "ModuleSubjectId" });
            DropIndex("dbo.TeachingUnits", new[] { "SemesterId" });
            DropIndex("dbo.TeachingUnits", new[] { "AcademicYearId" });
            DropIndex("dbo.TeachingUnits", new[] { "TeacherId" });
            DropIndex("dbo.Grades", new[] { "TeachingUnitId" });
            DropIndex("dbo.Grades", new[] { "InscriptionId" });
            DropPrimaryKey("dbo.ModuleSubjects");
            DropColumn("dbo.ModuleSubjects", "Id");
            DropColumn("dbo.Inscriptions", "UpdatedAt");
            DropColumn("dbo.Inscriptions", "CreatedAt");
            DropTable("dbo.TeachingUnits");
            DropTable("dbo.Grades");
            AddPrimaryKey("dbo.ModuleSubjects", new[] { "ModuleId", "SubjectId" });
        }
    }
}
