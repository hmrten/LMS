using LMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace LMS.DataAccess
{
    public class LMSContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
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
        }

        public void Seed()
        {
            AppUsers.AddOrUpdate(u => u.Id,
                new AppUser { Id = 1, FirstName = "Anders", LastName = "Andersson" });

            Teachers.AddOrUpdate(t => t.Id,
                new Teacher { Id = 1, AppUser_Id = 1 });

            Subjects.AddOrUpdate(s => s.Id,
                new Subject { Id = 1, Title = "Maths 1", Description = "Algebra and Calculus" });

            SaveChanges();

            var subj = Subjects.Find(1);
            var teacher = Teachers.Find(1);
            teacher.Subjects.Add(subj);
        }
    }
}