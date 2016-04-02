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
		public DbSet<ScheduleType> ScheduleTypes { get; set; }
		public DbSet<Schedule> Schedules { get; set; }
		public DbSet<Assignment> Assignments { get; set; }
		public DbSet<StudentAssignment> StudentAssignments { get; set; }
		public DbSet<Grading> Gradings { get; set; }

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

			OneToManyRelation<Schedule, ScheduleType>(mb, r => r.Type, m => m.Schedules, k => k.ScheduleType_Id);
			OneToManyRelation<Schedule, Group>(mb, r => r.Group, m => m.Schedules, k => k.Group_Id);
			OneToManyRelation<Schedule, Subject>(mb, r => r.Subject, m => m.Schedules, k => k.Subject_Id);
			OneToManyRelation<Schedule, Teacher>(mb, r => r.Author, m => m.Schedules, k => k.Author_Id);
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

        private void SeedSchedule()
        {
            Schedules.AddOrUpdate(s => s.Id,
                new Schedule
                {
                    Id = 1,
                    ScheduleType_Id = 1,
                    Group_Id = 1,
                    Subject_Id = 1,
                    Author_Id = 1,
                    Assignment_Id = null,
                    DateStart = DateTime.Now,
                    DateEnd = new DateTime(2016, 4, 4),
                    Description = "Study hard"
                },
                new Schedule
                {
                    Id = 2,
                    ScheduleType_Id = 1,
                    Group_Id = 1,
                    Subject_Id = 2,
                    Author_Id = 2,
                    Assignment_Id = null,
                    DateStart = DateTime.Now,
                    DateEnd = new DateTime(2016, 4, 5),
                    Description = "Study hard again"
                },
                new Schedule
                {
                    Id = 3,
                    ScheduleType_Id = 3,
                    Group_Id = 1,
                    Subject_Id = 2,
                    Author_Id = 3,
                    Assignment_Id = null,
                    DateStart = DateTime.Now,
                    DateEnd = new DateTime(2016, 4, 8),
                    Description = "Time for a meeting"
                });
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
			
			/*Subjects.AddOrUpdate(s => s.Id,
				new Subject { Id = 1, Title = "Hemkunskap", Description = "Liten kort beskrivning" },
				new Subject { Id = 2, Title = "Engelska", Description = "Liten kort beskrivning" },
				new Subject { Id = 3, Title = "Samhällskunskap", Description = "Liten kort beskrivning" }
				);

			ScheduleTypes.AddOrUpdate(s => s.Id,
				new ScheduleType { Id = 1, Name = "Inlämning" },
				new ScheduleType { Id = 2, Name = "Självstudier" },
				new ScheduleType { Id = 3, Name = "Eget arbete" },
				new ScheduleType { Id = 4, Name = "Träff" }
				);

			Groups.AddOrUpdate(t => t.Id,
				new Group { Id = 1, Name = "Samhällsvetenskap"},
				new Group { Id = 2, Name = "Teknik"},
				new Group { Id = 3, Name = "Ekonomi" },
				new Group { Id = 4, Name = "Naturvetenskap" }
				);*/

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
				Students.Find(i+10).Group_Id = 2;
			}
			SaveChanges();

			Subjects.AddOrUpdate(s => s.Id,
				new Subject { Id = 1, Name = "Ekonomi 1", Description = "Info om Ekonomi 1" },
				new Subject { Id = 2, Name = "Ekonomi 2", Description = "Info om Ekonomi 2" },
				new Subject { Id = 3, Name = "Historia 1", Description = "Info om Historia 1" },
				new Subject { Id = 4, Name = "Fysik 1", Description = "Info om Fysik 1" },
				new Subject { Id = 5, Name = "Fysik 2", Description = "Info om Fysik 2" }
				);
			ScheduleTypes.AddOrUpdate(s => s.Id,
				new ScheduleType { Id = 1, Name = "Studier" },
				new ScheduleType { Id = 2, Name = "Uppgift" },
				new ScheduleType { Id = 3, Name = "Möte" }
				);
			SaveChanges();

			Teachers.Find(1).Subjects.Add(Subjects.Find(1));
			Teachers.Find(1).Subjects.Add(Subjects.Find(2));
			Teachers.Find(2).Subjects.Add(Subjects.Find(3));
			Teachers.Find(3).Subjects.Add(Subjects.Find(4));
			Teachers.Find(4).Subjects.Add(Subjects.Find(5));
			SaveChanges();

            SeedSchedule();
		}

		public static LMSContext Create()
		{
			return new LMSContext();
		}
	}
}