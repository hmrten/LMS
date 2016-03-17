using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class ScheduleType
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(64), Index(IsUnique=true)]
        public string Name { get; set; }
    }
}