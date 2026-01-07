namespace systeme_gestion_isga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        PasswordHash = c.String(),
                        Role = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "UserID", "dbo.Users");
            DropForeignKey("dbo.Teachers", "UserID", "dbo.Users");
            DropIndex("dbo.Teachers", new[] { "UserID" });
            DropIndex("dbo.Students", new[] { "UserID" });
            DropTable("dbo.Teachers");
            DropTable("dbo.Users");
            DropTable("dbo.Students");
        }
    }
}
