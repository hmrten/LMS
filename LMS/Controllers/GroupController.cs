using LMS.DataAccess;
using LMS.Models;
using System;
using System.Collections.Generic;
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
                    students = g.Students.Count()
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
        public HttpStatusCodeResult Create(string name)
        {
            var group = new Group { Name = name };
            db.Groups.Add(group);
            db.SaveChanges();
            return new HttpStatusCodeResult(200, String.Format("{0}:{1}", group.Id, group.Name));
        }

        [HttpDelete]
        public HttpStatusCodeResult Delete(int id)
        {
            var group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return group == null ?
                HttpNotFound() :
                new HttpStatusCodeResult(200, String.Format("{0}:{1}", group.Id, group.Name));
        }
    }
}