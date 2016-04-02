using LMS.DataAccess;
using LMS.Models;
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
					fname = s.User.FirstName,
                    lname = s.User.LastName
				});
			return Json(q.ToList(), JsonRequestBehavior.AllowGet);
		}

        public JsonResult Schedule()
        {
            // type 1: studier
            // type 2: uppgift
            // type 3: mote

            // subject: 1-5

            // teacher: 1-5

            // groups: 1-2

            var q = db.Schedules.Select(s => new {
                id = s.Id,
                type_id = s.ScheduleType_Id,
                type_name = s.Type.Name,
                group_id = s.Group_Id,
                group_name = s.Group.Name,
                subject_id = s.Subject_Id,
                subject_name = s.Subject.Name,
                date_start = s.DateStart,
                date_end = s.DateEnd,
                description = s.Description
            });

            return Json(q, JsonRequestBehavior.AllowGet);
        }
    }
}