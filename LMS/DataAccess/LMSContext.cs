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

            /*mb.Entity<Schedule>()
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
                .WillCascadeOnDelete(false);*/
        }

        private void TryAddToRole(AppUserManager manager, int userId, string role)
        {
            if (!manager.IsInRole(userId, role))
                manager.AddToRole(userId, role);
        }

        private void AddUserAndRole(AppUserManager manager, string name, string role)
        {
            var user = manager.FindByName(name);
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = name,
                    Email = name + "@" + name + ".com",
                    FirstName = name,
                    LastName = name
                };
                var res = manager.Create(user, name);
                if (!res.Succeeded)
                {
                    throw new Exception(res.Errors.Aggregate("", (a, b) => a + b + "\n"));
                }
            }

            TryAddToRole(manager, user.Id, role);
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

            AddUserAndRole(userManager, "admin", "admin");
            AddUserAndRole(userManager, "teacher", "teacher");
            AddUserAndRole(userManager, "student", "student");
        }

        public static LMSContext Create()
        {
            return new LMSContext();
        }
    }
}