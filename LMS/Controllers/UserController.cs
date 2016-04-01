using LMS.DataAccess;
using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class UserController : Controller
    {
		private LMSContext db = new LMSContext();
		private AppUserManager userManager;

		public UserController()
		{
			userManager = AppUserManager.Create(new IntUserStore(db));
		}

		public JsonResult GetRoles()
		{
			var q = db.Roles.Select(r => 
				new
				{
					id = r.Id,
					name = r.Name
				});

			return Json(q.ToList(), JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetUser(int id)
		{
			var role = userManager.GetRoles(id).First();
			var user = db.Users.Where(s => s.Id == id).Select(s =>
				new
				{
					id = s.Id,
					fname = s.FirstName,
					lname = s.LastName,
					email = s.Email,
					phone = s.PhoneNumber,
					role = role
				}).SingleOrDefault();

			return Json(user, JsonRequestBehavior.AllowGet);
		}

		public JsonResult List()
		{
			var q = db.Users.Select(s =>
				new
				{
					id = s.Id,
					fname = s.FirstName,
					lname = s.LastName
				});

			return Json(q.ToList(), JsonRequestBehavior.AllowGet);
		}

    }
}