using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace LMS.DataAccess
{
    public class LMSContext : IdentityDbContext<User, IntRole, int, IntUserLogin, IntUserRole, IntUserClaim>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Grading> Gradings { get; set; }
        public DbSet<Upload> Uploads { get; set; }

        public LMSContext() : base("LMS") { }

        private static void OneToManyRelation<TEntity, TRequired>(DbModelBuilder mb,
            Expression<Func<TEntity, TRequired>> hasRequiredExpr,
            Expression<Func<TRequired, ICollection<TEntity>>> withManyExpr,
            Expression<Func<TEntity, int>> foreignKeyExpr)
            where TEntity : class
            where TRequired : class
        {
            mb.Entity<TEntity>()
                .HasRequired(hasRequiredExpr)
                .WithMany(withManyExpr)
                .HasForeignKey(foreignKeyExpr)
                .WillCascadeOnDelete(false);
        }

        // NOTE: For final presentation, talk about Fluent API
        protected override void OnModelCreating(DbModelBuilder mb)
        {
            base.OnModelCreating(mb);

            //OneToManyRelation<Schedule, ScheduleType>(mb, r => r.Type, m => m.Schedules, k => k.ScheduleType_Id);
            OneToManyRelation<Schedule, Group>(mb, r => r.Group, m => m.Schedules, k => k.Group_Id);
            //OneToManyRelation<Schedule, Subject>(mb, r => r.Subject, m => m.Schedules, k => k.Subject_Id);
            //OneToManyRelation<Schedule, Teacher>(mb, r => r.Author, m => m.Schedules, k => k.Author_Id);

            mb.Entity<Upload>()
                .HasMany(u => u.Assignments)
                .WithOptional(a => a.Upload)
                .HasForeignKey(a => a.Upload_Id)
                .WillCascadeOnDelete(false);
            mb.Entity<Upload>()
                .HasMany(u => u.Submissions)
                .WithOptional(a => a.Upload)
                .HasForeignKey(a => a.Upload_Id)
                .WillCascadeOnDelete(false);
        }

        private void TryAddToRole(AppUserManager manager, int userId, string role)
        {
            if (!manager.IsInRole(userId, role))
                manager.AddToRole(userId, role);
        }

        private User AddUserAndRole(AppUserManager manager, string userName, string password, string firstName, string lastName, string role)
        {
            var user = manager.FindByName(userName);
            if (user == null)
            {
                user = new User
                {
                    UserName = userName,
                    Email = userName + "@" + userName + ".com",
                    FirstName = firstName,
                    LastName = lastName
                };
                var res = manager.Create(user, password);
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.Aggregate("", (a, b) => a + b + "\n"));
                }
            }

            TryAddToRole(manager, user.Id, role);
            return user;
        }

        private Teacher AddTeacher(AppUserManager manager, int id, string userName, string password, string firstName, string lastName)
        {
            var user = AddUserAndRole(manager, userName, password, firstName, lastName, "teacher");
            var teacher = new Teacher { Id = id, User_Id = user.Id };
            Teachers.AddOrUpdate(t => t.Id, teacher);
            SaveChanges();
            return teacher;
        }
        private Student AddStudent(AppUserManager manager, int id, string userName, string password, string firstName, string lastName)
        {
            var user = AddUserAndRole(manager, userName, password, firstName, lastName, "student");
            var student = new Student { Id = id, User_Id = user.Id };
            Students.AddOrUpdate(s => s.Id, student);
            SaveChanges();
            return student;
        }

        private void AddSchedule(int id, ScheduleType type, int gid, DateTime start, DateTime end, string description, int subId)
        {
            var sched = new Schedule { Id = id, Type = type, Group_Id = gid, DateStart = start, DateEnd = end, Description = description };
            if (Schedules.Find(id) != null)
                return;
            Schedules.Add(sched);
            Subjects.Find(subId).Schedules.Add(sched);
        }

        private void SeedSchedules()
        {
            AddSchedule(1, ScheduleType.Studies, 1, new DateTime(2016, 4, 4, 12, 0, 0), new DateTime(2016, 4, 4, 13, 0, 0), "Study hard", 1);
            AddSchedule(2, ScheduleType.Studies, 1, new DateTime(2016, 4, 4, 13, 0, 0), new DateTime(2016, 4, 4, 14, 0, 0), "Study hard", 2);
            AddSchedule(3, ScheduleType.Meeting, 1, new DateTime(2016, 4, 4, 15, 0, 0), new DateTime(2016, 4, 4, 16, 0, 0), "Dags for möte", 1);
            AddSchedule(4, ScheduleType.Studies, 1, new DateTime(2016, 4, 8, 7, 0, 0), new DateTime(2016, 4, 8, 9, 0, 0), "Study hard", 3);
            AddSchedule(5, ScheduleType.Studies, 1, new DateTime(2016, 4, 11, 12, 0, 0), new DateTime(2016, 4, 11, 17, 0, 0), "Study hard", 3);

            AddSchedule(6, ScheduleType.Studies, 2, new DateTime(2016, 4, 5, 6, 0, 0), new DateTime(2016, 4, 5, 12, 0, 0), "Study hard", 4);
            AddSchedule(7, ScheduleType.Studies, 2, new DateTime(2016, 4, 7, 6, 0, 0), new DateTime(2016, 4, 7, 9, 0, 0), "Study hard", 5);
            AddSchedule(8, ScheduleType.Studies, 2, new DateTime(2016, 4, 7, 10, 0, 0), new DateTime(2016, 4, 7, 12, 0, 0), "Study hard", 4);
            AddSchedule(9, ScheduleType.Meeting, 2, new DateTime(2016, 4, 14, 13, 0, 0), new DateTime(2016, 4, 14, 14, 0, 0), "Dags for möte", 5);

            SaveChanges();
        }

        private void SeedFiles()
        {
            Uploads.AddOrUpdate(u => u.Id,
                new Upload { Id = 1, User_Id = 2, FilePath = "fil1.pdf" },
                new Upload { Id = 2, User_Id = 2, FilePath = "fil2.pdf" },
                new Upload { Id = 3, User_Id = 2, FilePath = "fil3.pdf" },
                new Upload { Id = 4, User_Id = 2, FilePath = "fil4.pdf" },
                new Upload { Id = 5, User_Id = 2, FilePath = "fil5.pdf" }
            );

            Assignments.AddOrUpdate(a => a.Id,
                new Assignment { Id = 1, Subject_Id = 1, Title = "Uppgift med id 1", Description = "Beskrivning 1", DateStart = new DateTime(2016, 3, 13), DateEnd = new DateTime(2016, 5, 15), Upload_Id = 1 },
                new Assignment { Id = 2, Subject_Id = 1, Title = "Uppgift med id 2", Description = "Beskrivning 2", DateStart = new DateTime(2016, 4, 20), DateEnd = new DateTime(2016, 6, 10), Upload_Id = 1 },
                new Assignment { Id = 3, Subject_Id = 3, Title = "Uppgift med id 3", Description = "Beskrivning 3", DateStart = new DateTime(2016, 3, 13), DateEnd = new DateTime(2016, 5, 15), Upload_Id = 1 },
                new Assignment { Id = 4, Subject_Id = 4, Title = "Uppgift med id 4", Description = "Beskrivning 4", DateStart = new DateTime(2016, 3, 13), DateEnd = new DateTime(2016, 5, 15), Upload_Id = 1 },
                new Assignment { Id = 5, Subject_Id = 5, Title = "Uppgift med id 5", Description = "Beskrivning 5", DateStart = new DateTime(2016, 3, 14), DateEnd = new DateTime(2016, 5, 10), Upload_Id = 2 }
                );

            Gradings.AddOrUpdate(g => g.Id,
                new Grading { Id = 1, Teacher_Id = 1, Date = new DateTime(2016, 6, 15), Feedback = "Bra jobbat!!", Grade = 1 }
                );

            Submissions.AddOrUpdate(s => s.Id,
                new Submission { Id = 1, Assignment_Id = 1, Student_Id = 1, Upload_Id = 3, SubmitDate = new DateTime(2016, 5, 14), Grading_Id = 1 },
                new Submission { Id = 2, Assignment_Id = 1, Student_Id = 2, Upload_Id = 4, SubmitDate = new DateTime(2016, 5, 13) },
                new Submission { Id = 3, Assignment_Id = 2, Student_Id = 3, Upload_Id = 5, SubmitDate = new DateTime(2016, 5, 29) },
                new Submission { Id = 4, Assignment_Id = 2, Student_Id = 1, Upload_Id = 3, SubmitDate = new DateTime(2016, 5, 14) }
                );

            SaveChanges();
        }

        public void Seed()
        {
            var roleManager = new RoleManager<IntRole, int>(new IntRoleStore(this));
            var userManager = AppUserManager.Create(new IntUserStore(this));

            string[] roleNames = { "admin", "teacher", "student" };
            foreach (var name in roleNames)
            {
                if (!Roles.Any(r => r.Name == name))
                    roleManager.Create(new IntRole { Name = name });
            }

            AddUserAndRole(userManager, "admin", "admin", "admin", "admin", "admin");

            AddTeacher(userManager, 1, "t01", "t", "Anders", "Andersson");
            AddTeacher(userManager, 2, "t02", "t", "Lena", "Olsson");
            AddTeacher(userManager, 3, "t03", "t", "Bertil", "Larsson");
            AddTeacher(userManager, 4, "t04", "t", "Eva", "Nilsson");
            AddTeacher(userManager, 5, "t05", "t", "Sara", "Jonsson");

            // Klass 1
            AddStudent(userManager, 1, "s01", "s", "Pelle", "Larsson");
            AddStudent(userManager, 2, "s02", "s", "Ludvig", "Jonasson");
            AddStudent(userManager, 3, "s03", "s", "Ida", "Claesson");
            AddStudent(userManager, 4, "s04", "s", "Sten", "Lundström");
            AddStudent(userManager, 5, "s05", "s", "Bernt", "Håkansson");
            AddStudent(userManager, 6, "s06", "s", "Filip", "Hellström");
            AddStudent(userManager, 7, "s07", "s", "Jenny", "Olofsson");
            AddStudent(userManager, 8, "s08", "s", "Hans", "Hansson");
            AddStudent(userManager, 9, "s09", "s", "Hugo", "Karlsson");
            AddStudent(userManager, 10, "s10", "s", "Alexandra", "Berg");

            // Klass 2
            AddStudent(userManager, 11, "s11", "s", "Lars", "Danielsson");
            AddStudent(userManager, 12, "s12", "s", "Jonas", "Bergqvist");
            AddStudent(userManager, 13, "s13", "s", "Henrik", "Svensson");
            AddStudent(userManager, 14, "s14", "s", "Oliver", "Pettersson");
            AddStudent(userManager, 15, "s15", "s", "Åke", "Sjöberg");
            AddStudent(userManager, 16, "s16", "s", "Elisabeth", "Abrahamsson");
            AddStudent(userManager, 17, "s17", "s", "Thomas", "Göransson");
            AddStudent(userManager, 18, "s18", "s", "Håkan", "Sundström");
            AddStudent(userManager, 19, "s19", "s", "Axel", "Ek");
            AddStudent(userManager, 20, "s20", "s", "Jonathan", "Bergström");

            Groups.AddOrUpdate(g => g.Id,
                new Group { Id = 1, Name = "Klass 1", Teacher_Id = 1 },
                new Group { Id = 2, Name = "Klass 2", Teacher_Id = 2 }
                );
            SaveChanges();

            for (int i = 1; i <= 10; ++i)
            {
                Students.Find(i).Group_Id = 1;
                Students.Find(i + 10).Group_Id = 2;
            }
            SaveChanges();

            Subjects.AddOrUpdate(s => s.Id,
                new Subject { Id = 1, Name = "Ekonomi 1", Description = "Info om Ekonomi 1" },
                new Subject { Id = 2, Name = "Ekonomi 2", Description = "Info om Ekonomi 2" },
                new Subject { Id = 3, Name = "Historia 1", Description = "Info om Historia 1" },
                new Subject { Id = 4, Name = "Fysik 1", Description = "Info om Fysik 1" },
                new Subject { Id = 5, Name = "Fysik 2", Description = "Info om Fysik 2" }
                );
            SaveChanges();

            Teachers.Find(1).Subjects.Add(Subjects.Find(1));
            Teachers.Find(1).Subjects.Add(Subjects.Find(2));
            Teachers.Find(2).Subjects.Add(Subjects.Find(3));
            Teachers.Find(3).Subjects.Add(Subjects.Find(4));
            Teachers.Find(4).Subjects.Add(Subjects.Find(5));

            Groups.Find(1).Subjects.Add(Subjects.Find(1));
            Groups.Find(1).Subjects.Add(Subjects.Find(2));
            Groups.Find(1).Subjects.Add(Subjects.Find(3));
            Groups.Find(2).Subjects.Add(Subjects.Find(4));
            Groups.Find(2).Subjects.Add(Subjects.Find(5));

            SaveChanges();

            SeedSchedules();

            //SeedFiles();
        }

        public static LMSContext Create()
        {
            return new LMSContext();
        }
    }
}