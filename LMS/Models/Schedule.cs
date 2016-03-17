using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        public int ScheduleType_Id { get; set; }

        public int Group_Id { get; set; }

        public int Subject_Id { get; set; }

        public int Author_Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        [ForeignKey("ScheduleType_Id")]
        public virtual ScheduleType ScheduleType { get; set; }

        [ForeignKey("Group_Id")]
        public virtual Group Group { get; set; }

        [ForeignKey("Subject_Id")]
        public virtual Subject Subject { get; set; }

        [ForeignKey("Author_Id")]
        public virtual Teacher Author { get; set; }
    }
}