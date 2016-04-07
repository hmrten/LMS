using LMS.DataAccess;
using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

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
            var q = from s in db.Schedules
                    where s.Group_Id == id
                    select new
                    {
                        id = s.Id,
                        type = s.Type.ToString(),
                        group_id = s.Group_Id,
                        group_name = s.Group.Name,
                        subject_name = s.Subjects.FirstOrDefault().Name,
                        date_start = s.DateStart,
                        date_end = s.DateEnd,
                        description = s.Description
                    };
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        // group id
        public JsonResult ScheduleTree(int id)
        {
            var q = from sched in db.Schedules
                    where sched.Group_Id == id
                    group sched by sched.DateStart.Month into monthGroup
                    orderby monthGroup.Key
                    select new
                    {
                        month = monthGroup.Key - 1,
                        days = from mg in monthGroup
                               group mg by mg.DateStart.Day into dayGroup
                               orderby dayGroup.Key
                               select new
                               {
                                   day = dayGroup.Key,
                                   schedules = (from dg in dayGroup
                                                select new
                                                {
                                                    id = dg.Id,
                                                    type = dg.Type,
                                                    subject_name = dg.Subjects.FirstOrDefault().Name,
                                                    date_start = dg.DateStart,
                                                    date_end = dg.DateEnd,
                                                    description = dg.Description
                                                }).OrderByDescending(s => s.date_start)
                               }
                    };
            var o = new { name = db.Groups.Find(id).Name, tree = q };
            return Json(o, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StudentSchedule()
        {
            var userId = HttpContext.User.Identity.GetUserId<int>();
            var student = db.Students.Where(s => s.User_Id == userId).SingleOrDefault();
            return ScheduleTree(student.Group_Id.Value);
        }

        // assignments for group id
        public JsonResult Assignments(int id)
        {
            var q = from g in db.Groups
                    where g.Id == id
                    select g.Subjects into gs
                    from s in gs
                    select s.Assignments into assigns
                    from a in assigns
                    select a into assign
                    group assign by new
                    {
                        id = assign.Subject_Id,
                        name = assign.Subject.Name
                    } into grp
                    orderby grp.Key.id
                    select new
                    {
                        subject = grp.Key,
                        assignments = grp.Select(x => new
                        {
                            id = x.Id,
                            title = x.Title,
                            desc = x.Description,
                            filepath = x.Upload.FilePath,
                            date_start = x.DateStart,
                            date_end = x.DateEnd,
                            submissions = from sub in x.Submissions
                                          where sub.Assignment_Id == x.Id
                                          select new
                                          {
                                              student = new { id = sub.Student_Id, name = sub.Student.User.FirstName + " " + sub.Student.User.LastName },
                                              comment = sub.Comment,
                                              date = sub.SubmitDate,
                                              filepath = sub.Upload.FilePath
                                          }
                        })
                    };
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Groups()
        {
            var q = db.Groups.Select(g => new
            {
                id = g.Id,
                name = g.Name,
                student_count = g.Students.Count
            });
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GroupName(int id)
        {
            return Json(db.Groups.Find(id).Name, JsonRequestBehavior.AllowGet);
        }
    }
}