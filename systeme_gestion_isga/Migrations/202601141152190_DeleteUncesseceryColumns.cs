namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteUncesseceryColumns : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inscriptions", new[] { "AcademicYearId" });
            DropIndex("dbo.Inscriptions", new[] { "ProgramAcademicYearId" });
            RenameColumn(table: "dbo.Inscriptions", name: "AcademicYearId", newName: "AcademicYear_Id");
            RenameColumn(table: "dbo.Inscriptions", name: "ProgramAcademicYearId", newName: "ProgramAcademicYear_Id");
            AlterColumn("dbo.Inscriptions", "AcademicYear_Id", c => c.Int());
            AlterColumn("dbo.Inscriptions", "ProgramAcademicYear_Id", c => c.Int());
            CreateIndex("dbo.Inscriptions", "ProgramAcademicYear_Id");
            CreateIndex("dbo.Inscriptions", "AcademicYear_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inscriptions", new[] { "AcademicYear_Id" });
            DropIndex("dbo.Inscriptions", new[] { "ProgramAcademicYear_Id" });
            AlterColumn("dbo.Inscriptions", "ProgramAcademicYear_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inscriptions", "AcademicYear_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Inscriptions", name: "ProgramAcademicYear_Id", newName: "ProgramAcademicYearId");
            RenameColumn(table: "dbo.Inscriptions", name: "AcademicYear_Id", newName: "AcademicYearId");
            CreateIndex("dbo.Inscriptions", "ProgramAcademicYearId");
            CreateIndex("dbo.Inscriptions", "AcademicYearId");
        }
    }
}
