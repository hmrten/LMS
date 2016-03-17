namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 128),
                        LastName = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        Teacher_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id, cascadeDelete: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppUser_Id = c.Int(nullable: false),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppUser_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id, cascadeDelete: true)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 64),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Title, unique: true);
            
            CreateTable(
                "dbo.SubjectTeachers",
                c => new
                    {
                        Subject_Id = c.Int(nullable: false),
                        Teacher_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subject_Id, t.Teacher_Id })
                .ForeignKey("dbo.Subjects", t => t.Subject_Id, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id, cascadeDelete: true)
                .Index(t => t.Subject_Id)
                .Index(t => t.Teacher_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.SubjectTeachers", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.SubjectTeachers", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.Teachers", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Students", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Students", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.SubjectTeachers", new[] { "Teacher_Id" });
            DropIndex("dbo.SubjectTeachers", new[] { "Subject_Id" });
            DropIndex("dbo.Subjects", new[] { "Title" });
            DropIndex("dbo.Teachers", new[] { "AppUser_Id" });
            DropIndex("dbo.Students", new[] { "Group_Id" });
            DropIndex("dbo.Students", new[] { "AppUser_Id" });
            DropIndex("dbo.Groups", new[] { "Teacher_Id" });
            DropIndex("dbo.Groups", new[] { "Name" });
            DropTable("dbo.SubjectTeachers");
            DropTable("dbo.Subjects");
            DropTable("dbo.Teachers");
            DropTable("dbo.Students");
            DropTable("dbo.Groups");
            DropTable("dbo.AppUsers");
        }
    }
}
