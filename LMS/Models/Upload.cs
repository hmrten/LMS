using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Upload
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FilePath { get; set; }

        public int User_Id { get; set; }

        [ForeignKey("User_Id")]
        public virtual User User { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Submission> Submissions { get; set; }

        public Upload()
        {
            Assignments = new List<Assignment>();
            Submissions = new List<Submission>();
        }
    }
}