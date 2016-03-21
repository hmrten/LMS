using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace LMS.DataAccess
{
    public class LMSContext : IdentityDbContext<AppUser, IntRole, int, IntUserLogin, IntUserRole, IntUserClaim>
    {
        //public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ScheduleType> ScheduleTypes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<StudentAssignment> StudentAssignments { get; set; }
        public DbSet<Grading> Gradings { get; set; }

        public LMSContext() : base("LMS") { }

        // NOTE: For final presentation, talk about Fluent API
        protected override void OnModelCreating(DbModelBuilder mb)
        {
            // NOTE: Have to setup tentity relations manually because we have circular depdencies
            //       and we don't want on cascade delete with the foreign keys
            mb.Entity<Schedule>()
                .HasRequired(s => s.Type)
                .WithMany(t => t.Schedules)
                .HasForeignKey(s => s.ScheduleType_Id)
                .WillCascadeOnDelete(false);

            mb.Entity<Schedule>()
                .HasRequired(s => s.Group)
                .WithMany(g => g.Schedules)
                .HasForeignKey(s => s.Group_Id)
                .WillCascadeOnDelete(false);

            mb.Entity<Schedule>()
                .HasRequired(s => s.Subject)
                .WithMany(s => s.Schedules)
                .HasForeignKey(s => s.Subject_Id)
                .WillCascadeOnDelete(false);

            mb.Entity<Schedule>()
                .HasRequired(s => s.Author)
                .WithMany(a => a.Schedules)
                .HasForeignKey(s => s.Author_Id)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(mb);
        }

        public void Seed()
        {
            //AppUsers.AddOrUpdate(u => u.Id,
            //    new AppUser { Id = 1, FirstName = "Anders", LastName = "Andersson" });

            //Teachers.AddOrUpdate(t => t.Id,
            //    new Teacher { Id = 1, AppUser_Id = 1 });

            //Subjects.AddOrUpdate(s => s.Id,
            //    new Subject { Id = 1, Title = "Maths 1", Description = "Algebra and Calculus" });

            //SaveChanges();

            //var subj = Subjects.Find(1);
            //var teacher = Teachers.Find(1);
            //teacher.Subjects.Add(subj);

            if (!Roles.Any(r => r.Name == "admin"))
            {
                var rm = new RoleManager<IntRole, int>(new IntRoleStore(this));
                rm.Create(new IntRole { Name = "admin" });
            }

            if (!Users.Any(u => u.UserName == "admin"))
            {
                var store = new IntUserStore(this);
                var manager = AppUserManager.Create(store);
                var user = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "admin",
                    LastName = "admin"
                };
                var res = manager.Create(user, "admin");
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.Aggregate("", (a, b) => a + b + "\n"));
                }
                //SaveChanges();
            }
            else
            {
                var manager = AppUserManager.Create(new IntUserStore(this));
                manager.AddToRole(1, "admin");
            }
        }

        public static LMSContext Create()
        {
            return new LMSContext();
        }
    }
}