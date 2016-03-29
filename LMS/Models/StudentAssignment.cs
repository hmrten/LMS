using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class StudentAssignment
    {
        [Key]
        public int Id { get; set; }

        public int Student_Id { get; set; }

        public int Assignment_Id { get; set; }

        public int? Grading_Id { get; set; }

        [Required]
        public DateTime SubmitDate { get; set; }

        public string Comment { get; set; }

        [Required, StringLength(256)]
        public string FileName { get; set; }

        [ForeignKey("Student_Id")]
        public virtual Student Student { get; set; }

        [ForeignKey("Assignment_Id")]
        public virtual Assignment Assignment { get; set; }

        [ForeignKey("Grading_Id")]
        public virtual Grading Grading { get; set; }
    }
}