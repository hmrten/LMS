using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.ViewModels
{
    public class AssignmentViewModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
    }
}