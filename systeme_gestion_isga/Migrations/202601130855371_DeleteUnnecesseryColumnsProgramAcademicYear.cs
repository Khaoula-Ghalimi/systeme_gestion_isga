namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteUnnecesseryColumnsProgramAcademicYear : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProgramAcademicYears", "DisplayName");
            DropColumn("dbo.ProgramAcademicYears", "DisplayCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProgramAcademicYears", "DisplayCode", c => c.String(maxLength: 10));
            AddColumn("dbo.ProgramAcademicYears", "DisplayName", c => c.String(maxLength: 100));
        }
    }
}
