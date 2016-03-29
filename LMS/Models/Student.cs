using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public int User_Id { get; set; }

        public int? Group_Id { get; set; }

        [ForeignKey("User_Id")]
        public virtual User User { get; set; }

        [ForeignKey("Group_Id")]
        public virtual Group Group { get; set; }

        public virtual ICollection<StudentAssignment> StudentAssignments { get; set; }

        public Student()
        {
            StudentAssignments = new List<StudentAssignment>();
        }
    }
}