using LMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LMS.Controllers.API
{
    public class AssignmentController : Controller
    {
        public HttpStatusCodeResult Create(HttpPostedFileBase file)
        {
            var model = new JavaScriptSerializer().Deserialize(HttpContext.Request.Form["model"], typeof(AssignmentViewModel));
            return new HttpStatusCodeResult(500, "not implemented");
        }
    }
}