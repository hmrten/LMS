using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class TeacherController : Controller
    {
        private ViewResult SectionView(string action, string section)
        {
            return View("~/Views/Teacher/" + action + "/" + section + ".cshtml");
        }

        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

		//GET: Teacher/Subject
		public ViewResult Subject(string section)
		{
			return View("~/Views/Teacher/Subject/" + section + ".cshtml");
		}

        public ViewResult Task(string section)
        {
            return SectionView("Task", section);
        }
    }
}