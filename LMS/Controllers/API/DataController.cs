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
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ScheduleTypes()
        {
            var q = Enum.GetNames(typeof(ScheduleType));
            return Json(q, JsonRequestBehavior.AllowGet);
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
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Schedule(int id)
        {
            // type 1: studier
            // type 2: uppgift
            // type 3: mote

            // subject: 1-5

            // teacher: 1-5

            // groups: 1-2

            var q = from s in db.Schedules
                    where s.Group_Id == id
                    select new
                    {
                        id = s.Id,
                        //type_id = s.ScheduleType_Id,
                        //type_name = s.Type.Name,
                        type = s.Type.ToString(),
                        group_id = s.Group_Id,
                        group_name = s.Group.Name,
                        //subject_id = s.Subject_Id,
                        //subject_name = s.Subject.Name,
                        date_start = s.DateStart,
                        date_end = s.DateEnd,
                        description = s.Description
                    };
            return Json(q, JsonRequestBehavior.AllowGet);
        }
    }
}