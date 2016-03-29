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

        [StringLength(256)]
        public string FileName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<StudentAssignment> StudentAssignments { get; set; }

        public Assignment()
        {
            StudentAssignments = new List<StudentAssignment>();
        }
    }
}