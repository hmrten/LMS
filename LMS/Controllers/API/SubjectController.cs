using LMS.DataAccess;
using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{

    public class SubjectController : Controller
    {
		private LMSContext db = new LMSContext();

		[HttpPost]
		public HttpStatusCodeResult Create(string name, string description)
		{
			var subject = new Subject {Name = name, Description = description };

			db.Subjects.Add(subject);
			db.SaveChanges();

			return new HttpStatusCodeResult(200, "Ett nytt ämne med id: " + subject.Id.ToString() + "skapades");
		}

		[HttpPost]
		public HttpStatusCodeResult Update(int id, string name, string description)
		{
			var subject = db.Subjects.Where(s => s.Id == id).SingleOrDefault();

			subject.Name = name;
			subject.Description = description;

			db.SaveChanges();
			return new HttpStatusCodeResult(200, "Ett ämne med id: " + subject.Id.ToString() + "uppdaterades");
		}
		public JsonResult GetSubject(int id)
		{
			var subject = db.Subjects.Where(s => s.Id == id).Select(s =>
				new
				{
					id = s.Id,
					name = s.Name,
					description = s.Description
				}).SingleOrDefault();

			return Json(subject, JsonRequestBehavior.AllowGet);
		}

		public JsonResult List()
		{
			var q = db.Subjects.Select(s =>
				new
				{
					id = s.Id,
					name = s.Name,
					description = s.Description
				});

			return Json(q, JsonRequestBehavior.AllowGet);
		}

    }
}