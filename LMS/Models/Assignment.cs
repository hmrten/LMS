using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Assignment
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(128)]
        public string Title { get; set; }

        public string Description { get; set; }

        public int Upload_Id { get; set; }
        public int Subject_Id { get; set; }

        [ForeignKey("Upload_Id")]
        public virtual Upload Upload { get; set; }
        [ForeignKey("Subject_Id")]
        public virtual Subject Subject { get; set; }

        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

        public Assignment()
        {
            Submissions = new List<Submission>();
            Schedules = new List<Schedule>();
        }
    }
}