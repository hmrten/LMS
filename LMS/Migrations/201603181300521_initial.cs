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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gradings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Teacher_Id = c.Int(nullable: false),
                        Grade = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Feedback = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id, cascadeDelete: true)
                .Index(t => t.Teacher_Id);
            
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
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduleType_Id = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                        Subject_Id = c.Int(nullable: false),
                        Author_Id = c.Int(nullable: false),
                        Task_Id = c.Int(),
                        DateStart = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Author_Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .ForeignKey("dbo.Subjects", t => t.Subject_Id)
                .ForeignKey("dbo.Tasks", t => t.Task_Id)
                .ForeignKey("dbo.ScheduleTypes", t => t.ScheduleType_Id)
                .Index(t => t.ScheduleType_Id)
                .Index(t => t.Group_Id)
                .Index(t => t.Subject_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Task_Id);
            
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
                "dbo.StudentAssignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Student_Id = c.Int(nullable: false),
                        Task_Id = c.Int(nullable: false),
                        Grading_Id = c.Int(),
                        SubmitDate = c.DateTime(nullable: false),
                        Comment = c.String(),
                        FileName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gradings", t => t.Grading_Id)
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.Task_Id, cascadeDelete: true)
                .Index(t => t.Student_Id)
                .Index(t => t.Task_Id)
                .Index(t => t.Grading_Id);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 128),
                        FileName = c.String(maxLength: 256),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.ScheduleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
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
            DropForeignKey("dbo.Gradings", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.Schedules", "ScheduleType_Id", "dbo.ScheduleTypes");
            DropForeignKey("dbo.Schedules", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.Schedules", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.SubjectTeachers", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.SubjectTeachers", "Subject_Id", "dbo.Subjects");
            DropForeignKey("dbo.Schedules", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Groups", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.Students", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.StudentAssignments", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.StudentAssignments", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.StudentAssignments", "Grading_Id", "dbo.Gradings");
            DropForeignKey("dbo.Students", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Schedules", "Author_Id", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.SubjectTeachers", new[] { "Teacher_Id" });
            DropIndex("dbo.SubjectTeachers", new[] { "Subject_Id" });
            DropIndex("dbo.ScheduleTypes", new[] { "Name" });
            DropIndex("dbo.Subjects", new[] { "Title" });
            DropIndex("dbo.StudentAssignments", new[] { "Grading_Id" });
            DropIndex("dbo.StudentAssignments", new[] { "Task_Id" });
            DropIndex("dbo.StudentAssignments", new[] { "Student_Id" });
            DropIndex("dbo.Students", new[] { "Group_Id" });
            DropIndex("dbo.Students", new[] { "AppUser_Id" });
            DropIndex("dbo.Groups", new[] { "Teacher_Id" });
            DropIndex("dbo.Groups", new[] { "Name" });
            DropIndex("dbo.Schedules", new[] { "Task_Id" });
            DropIndex("dbo.Schedules", new[] { "Author_Id" });
            DropIndex("dbo.Schedules", new[] { "Subject_Id" });
            DropIndex("dbo.Schedules", new[] { "Group_Id" });
            DropIndex("dbo.Schedules", new[] { "ScheduleType_Id" });
            DropIndex("dbo.Teachers", new[] { "AppUser_Id" });
            DropIndex("dbo.Gradings", new[] { "Teacher_Id" });
            DropTable("dbo.SubjectTeachers");
            DropTable("dbo.ScheduleTypes");
            DropTable("dbo.Subjects");
            DropTable("dbo.Tasks");
            DropTable("dbo.StudentAssignments");
            DropTable("dbo.Students");
            DropTable("dbo.Groups");
            DropTable("dbo.Schedules");
            DropTable("dbo.Teachers");
            DropTable("dbo.Gradings");
            DropTable("dbo.AppUsers");
        }
    }
}
