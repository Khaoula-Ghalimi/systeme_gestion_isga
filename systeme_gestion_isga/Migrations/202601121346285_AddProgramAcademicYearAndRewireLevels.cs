namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProgramAcademicYearAndRewireLevels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Levels", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.Inscriptions", "ProgramId", "dbo.Programs");
            DropIndex("dbo.Inscriptions", new[] { "ProgramId" });
            DropIndex("dbo.Levels", new[] { "ProgramId" });
            CreateTable(
                "dbo.ProgramAcademicYears",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramId = c.Int(nullable: false),
                        AcademicYearId = c.Int(nullable: false),
                        DisplayName = c.String(maxLength: 100),
                        DisplayCode = c.String(maxLength: 10),
                        DurationInYearsOverride = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYears", t => t.AcademicYearId)
                .ForeignKey("dbo.Programs", t => t.ProgramId)
                .Index(t => new { t.ProgramId, t.AcademicYearId }, unique: true, name: "IX_Program_AcYear");
            
            AddColumn("dbo.Inscriptions", "ProgramAcademicYearId", c => c.Int(nullable: false));
            AddColumn("dbo.Levels", "ProgramAcademicYearId", c => c.Int(nullable: false));
            CreateIndex("dbo.Inscriptions", "ProgramAcademicYearId");
            CreateIndex("dbo.Levels", "ProgramAcademicYearId");
            AddForeignKey("dbo.Levels", "ProgramAcademicYearId", "dbo.ProgramAcademicYears", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inscriptions", "ProgramAcademicYearId", "dbo.ProgramAcademicYears", "Id");
            DropColumn("dbo.Inscriptions", "ProgramId");
            DropColumn("dbo.Levels", "ProgramId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Levels", "ProgramId", c => c.Int(nullable: false));
            AddColumn("dbo.Inscriptions", "ProgramId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Inscriptions", "ProgramAcademicYearId", "dbo.ProgramAcademicYears");
            DropForeignKey("dbo.Levels", "ProgramAcademicYearId", "dbo.ProgramAcademicYears");
            DropForeignKey("dbo.ProgramAcademicYears", "ProgramId", "dbo.Programs");
            DropForeignKey("dbo.ProgramAcademicYears", "AcademicYearId", "dbo.AcademicYears");
            DropIndex("dbo.ProgramAcademicYears", "IX_Program_AcYear");
            DropIndex("dbo.Levels", new[] { "ProgramAcademicYearId" });
            DropIndex("dbo.Inscriptions", new[] { "ProgramAcademicYearId" });
            DropColumn("dbo.Levels", "ProgramAcademicYearId");
            DropColumn("dbo.Inscriptions", "ProgramAcademicYearId");
            DropTable("dbo.ProgramAcademicYears");
            CreateIndex("dbo.Levels", "ProgramId");
            CreateIndex("dbo.Inscriptions", "ProgramId");
            AddForeignKey("dbo.Inscriptions", "ProgramId", "dbo.Programs", "Id");
            AddForeignKey("dbo.Levels", "ProgramId", "dbo.Programs", "Id", cascadeDelete: true);
        }
    }
}
