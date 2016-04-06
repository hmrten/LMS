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

        public int User_Id { get; set; }

        [ForeignKey("User_Id")]
        public virtual User User { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        public Teacher()
        {
            Subjects = new List<Subject>();
        }
    }
}