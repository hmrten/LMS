using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    //public class ScheduleType
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    [Required, StringLength(64), Index(IsUnique = true)]
    //    public string Name { get; set; }

    //    public virtual ICollection<Schedule> Schedules { get; set; }

    //    public ScheduleType()
    //    {
    //        Schedules = new List<Schedule>();
    //    }
    //}

    public enum ScheduleType
    {
        Studies = 0,
        Meeting = 1
    }

    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        public ScheduleType Type { get; set; }

        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }

        public string Description { get; set; }

        public int Group_Id { get; set; }

        [ForeignKey("Group_Id")]
        public virtual Group Group { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        public Schedule()
        {
            Subjects = new List<Subject>();
        }
    }
}