using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

		// GET: Schedule
		public ActionResult Schedule()
		{
			return View();
		}

		// GET: Group
		public ActionResult Group()
		{
			return View();
		}

		// GET: Task
		public ActionResult Task()
		{
			return View();
		}

		// GET: File
		public ActionResult File()
		{
			return View();
		}

    }
}