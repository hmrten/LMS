using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
	[AllowAnonymous]
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

		public ViewResult About()
		{
			return View();
		}

		public ViewResult Contact()
		{
			return View();
		}

        public ViewResult Login()
        {
            return View();
        }
    }
}