namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateProgramLevelSemester : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Levels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(maxLength: 10),
                        Order = c.Int(nullable: false),
                        ProgramId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programs", t => t.ProgramId, cascadeDelete: true)
                .Index(t => t.ProgramId);
            
            CreateTable(
                "dbo.Programs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(maxLength: 10),
                        Description = c.String(maxLength: 500),
                        DurationInYears = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Order = c.Int(nullable: false),
                        LevelId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Levels", t => t.LevelId, cascadeDelete: true)
                .Index(t => t.LevelId);
            
            AddColumn("dbo.Students", "StudentNumber", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Students", "CIN", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Students", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Students", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "Address", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Teachers", "EmployeeNumber", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Teachers", "CIN", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Teachers", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Teachers", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Teachers", "Address", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "PasswordHash", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 255));
            AlterColumn("dbo.Users", "Phone", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Semesters", "LevelId", "dbo.Levels");
            DropForeignKey("dbo.Levels", "ProgramId", "dbo.Programs");
            DropIndex("dbo.Semesters", new[] { "LevelId" });
            DropIndex("dbo.Levels", new[] { "ProgramId" });
            AlterColumn("dbo.Users", "Phone", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "LastName", c => c.String());
            AlterColumn("dbo.Users", "FirstName", c => c.String());
            AlterColumn("dbo.Users", "PasswordHash", c => c.String());
            AlterColumn("dbo.Users", "Username", c => c.String());
            DropColumn("dbo.Teachers", "Address");
            DropColumn("dbo.Teachers", "Gender");
            DropColumn("dbo.Teachers", "BirthDate");
            DropColumn("dbo.Teachers", "CIN");
            DropColumn("dbo.Teachers", "EmployeeNumber");
            DropColumn("dbo.Students", "Address");
            DropColumn("dbo.Students", "Gender");
            DropColumn("dbo.Students", "BirthDate");
            DropColumn("dbo.Students", "CIN");
            DropColumn("dbo.Students", "StudentNumber");
            DropTable("dbo.Semesters");
            DropTable("dbo.Programs");
            DropTable("dbo.Levels");
        }
    }
}
