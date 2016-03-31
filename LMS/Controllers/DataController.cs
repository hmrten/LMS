﻿using LMS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class DataController : Controller
    {
		private LMSContext db = new LMSContext();

        public JsonResult Subjects()
        {
			var q = db.Subjects.Select(s =>
				new
				{
					id = s.Id,
					name = s.Name,
					description = s.Description
				});
			return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }

		public JsonResult ScheduleTypes()
		{
			var q = db.ScheduleTypes.Select(s =>
				new
				{
					id = s.Id,
					name = s.Name
				});
			return Json(q.ToList(), JsonRequestBehavior.AllowGet);
		}

		public JsonResult Teachers()
		{
			var q = db.Teachers.Select(s =>
				new
				{
					id = s.Id,
					firstname = s.User.FirstName,
					lastname = s.User.LastName,
					email = s.User.Email,
					subjects = s.Subjects.Select(ss =>
						new
						{
							id = ss.Id,
							name = ss.Name
						}).ToList()
				});
			return Json(q.ToList(), JsonRequestBehavior.AllowGet);
		}
    }
}