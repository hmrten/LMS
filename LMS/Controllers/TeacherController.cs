using LMS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class TeacherController : Controller
    {
		private LMSContext db = new LMSContext();

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

		//POST:Teacher/Subject/Create
		[HttpPost]
		public ActionResult Create()
		{

			return View();
		}

    }
}