using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.ViewModels
{
    public class ScheduleViewModel
    {
        public int type_id { get; set; }
        public int group_id { get; set; }
        public int subject_id { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public string description { get; set; }
    }
}