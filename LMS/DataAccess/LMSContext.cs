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

        public LMSContext() : base("LMS") { }

        public void Seed()
        {
            AppUsers.AddOrUpdate(u => u.Id,
                new AppUser { Id = 1, FirstName = "Anders", LastName = "Andersson", Email = "anders@mail.com" });

            Teachers.AddOrUpdate(t => t.Id,
                new Teacher { Id = 1, AppUser_Id = 1 });

            Subjects.AddOrUpdate(s => s.Id,
                new Subject { Id = 1, Title = "Maths 1", Description = "Algebra and Calculus" });

            SaveChanges();

            var subj = Subjects.Find(1);
            Teachers.Find(1).Subjects.Add(subj);
        }
    }
}