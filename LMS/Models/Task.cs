using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        public int Schedule_Id { get; set; }

        [Required, StringLength(128)]
        public string Title { get; set; }

        public string FileName { get; set; }

        [ForeignKey("Schedule_Id")]
        public virtual Schedule Schedule { get; set; }

        public virtual ICollection<StudentAssignment> StudentAssignments { get; set; }
    }
}