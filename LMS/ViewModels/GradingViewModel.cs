using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.ViewModels
{
    public class GradingViewModel
    {
        public int sub_id { get; set; }
        public int grade { get; set; }
        public string feedback { get; set; }
    }
}