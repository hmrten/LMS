using LMS.DataAccess;
using LMS.Models;
using LMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers.API
{
    public class ScheduleController : Controller
    {
        private LMSContext db = new LMSContext();

        [HttpPost]
        public HttpStatusCodeResult Create(ScheduleViewModel model)
        {
            var sched = new Schedule
            {
                Type = model.type_id,
                Group_Id = model.group_id,
                DateStart = model.date_start,
                DateEnd = model.date_end,
                Description = model.description
            }; 
            db.Schedules.Add(sched);
            sched.Subjects.Add(db.Subjects.Find(model.subject_id));
            db.SaveChanges();

            var msg = String.Format("En ny händelse ({0}) med id: {1} har skapats", sched.Type.ToString(), sched.Id);
            return new HttpStatusCodeResult(200, msg);
        }
    }
}