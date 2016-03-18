using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Grading
    {
        [Key]
        public int Id { get; set; }

        public int Teacher_Id { get; set; }

        // NOTE: For now, this is a boolean, 0 means failed, 1 means approved
        public int Grade { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Feedback { get; set; }

        [ForeignKey("Teacher_Id")]
        public virtual Teacher Teacher { get; set; }
    }
}