using LMS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
	[Authorize(Roles="teacher")]
    public class TeacherController : Controller
    {
	    private LMSContext db = new LMSContext();
	
		//Här läggs laddningen av alla sidor till huvudmenyn (via razor)
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

	    //GET: Teacher/Subject
	    public ViewResult Subject()
	    {
			return View();
	    }

		//GET: Teacher/User
		public ViewResult User()
		{
			return View();
		}

        public ViewResult Assignment()
        {
            return View();
        }

        public ViewResult Group()
        {
            return View();
        }

	    [HttpPost]
	    public ActionResult Create()
	    {
		    return View();
	    }
    }
}