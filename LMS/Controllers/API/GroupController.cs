using LMS.DataAccess;
using LMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class GroupController : Controller
    {
        private LMSContext db = new LMSContext();

        public JsonResult List()
        {
            var q = db.Groups.Select(g =>
                new
                {
                    id = g.Id,
                    name = g.Name,
                    teacher_name = g.Teacher.User.FirstName + " " + g.Teacher.User.LastName,
                    students = g.Students.Count()
                });
            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FreeStudents()
        {
            var q = db.Students.Where(s => s.Group_Id == null).Select(s =>
                new
                {
                    id = s.Id,
                    fname = s.User.FirstName,
                    lname = s.User.LastName
                });
            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Details(int id)
        {
            var q = from g in db.Groups
                    where g.Id == id
                    select new
                    {
                        id = id,
                        name = g.Name,
                        teacher_id = g.Teacher_Id,
                        teacher_name = g.Teacher.User.FirstName + " " + g.Teacher.User.FirstName,
                        students = g.Students.Select(s =>
                        new
                        {
                            id = s.Id,
                            uname = s.User.UserName,
                            fname = s.User.FirstName,
                            lname = s.User.LastName,
                            email = s.User.Email,
                        })
                    };
            return Json(q.SingleOrDefault(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpStatusCodeResult Create(string name, int teacherId)
        {
            var group = new Group { Name = name, Teacher_Id = teacherId };
            db.Groups.Add(group);
            db.SaveChanges();
            return new HttpStatusCodeResult(200, String.Format("{0}:{1}", group.Id, group.Name));
        }

        [HttpPut]
        public HttpStatusCodeResult Update(int id, int teacher_id, int[] used, int[] free)
        {
            if (free != null)
            {
                foreach (var i in free)
                    db.Students.Find(i).Group_Id = null;
            }
            int n = 0;
            if (used != null)
            {
                foreach (var i in used)
                    db.Students.Find(i).Group_Id = id;
                n = used.Length;
            }
            var g = db.Groups.Find(id);
            g.Teacher_Id = teacher_id;
            db.SaveChanges();
            return new HttpStatusCodeResult(200, String.Format("{0} elever lades till i klass '{1}' med lärare '{2}'",
                n, g.Name, g.Teacher.User.FullName));
        }

        [HttpDelete]
        public HttpStatusCodeResult Delete(int id)
        {
            var group = db.Groups.Find(id);
            if (group.Students.Count != 0)
            {
                return new HttpStatusCodeResult(403, "Kan ej ta bort klass med elever.");
            }
            db.Groups.Remove(group);
            db.SaveChanges();
            return group == null ?
                HttpNotFound() :
                new HttpStatusCodeResult(200, String.Format("{0}:{1}", group.Id, group.Name));
        }
    }
}