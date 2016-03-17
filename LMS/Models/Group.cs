using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(64), Index(IsUnique=true)]
        public string Name { get; set; }

        public int Teacher_Id { get; set; }

        [ForeignKey("Teacher_Id")]
        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}