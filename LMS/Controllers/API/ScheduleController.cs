using LMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers.API
{
    public class ScheduleController : Controller
    {
        [HttpPost]
        public HttpStatusCodeResult Create(ScheduleViewModel model)
        {
            return new HttpStatusCodeResult(500, "not implemented");
        }
    }
}