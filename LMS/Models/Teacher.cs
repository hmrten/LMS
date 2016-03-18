using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        public int AppUser_Id { get; set; }

        [ForeignKey("AppUser_Id")]
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

        public Teacher()
        {
            Subjects = new List<Subject>();
            Schedules = new List<Schedule>();
        }
    }
}