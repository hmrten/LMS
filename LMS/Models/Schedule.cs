using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class ScheduleType
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(64), Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }

        public ScheduleType()
        {
            Schedules = new List<Schedule>();
        }
    }

    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        // Uses Fluent API configuration
        public int ScheduleType_Id { get; set; }
        public int Group_Id { get; set; }
        public int Subject_Id { get; set; }
        public int Author_Id { get; set; }

        public int? Task_Id { get; set; }

        [Required]
        public DateTime DateStart { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }

        public string Description { get; set; }

        // Uses Fluent API configuration
        public virtual ScheduleType Type { get; set; }
        public virtual Group Group { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Author { get; set; }

        [ForeignKey("Task_Id")]
        public virtual Task Task { get; set; }
    }
}