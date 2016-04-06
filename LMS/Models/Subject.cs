using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(64), Index(IsUnique=true)]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

        public Subject()
        {
            Teachers = new List<Teacher>();
            Schedules = new List<Schedule>();
            Assignments = new List<Assignment>();
            Groups = new List<Group>();
        }
    }
}