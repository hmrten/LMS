using LMS.DataAccess;
using LMS.Models;
using LMS.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Security.Claims;

namespace LMS.Controllers.API
{
    public class AssignmentController : Controller
    {
        private LMSContext db = new LMSContext();

        public AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }

        public JsonResult List()
        {
            var q = from g in db.Groups
                    group g by new { id = g.Id, name = g.Name } into gr
                    orderby gr.Key.id
                    select new
                    {
                        id = gr.Key.id,
                        name = gr.Key.name,
                        subjects = from gg in gr
                                   select gg.Subjects into gs
                                   from s in gs
                                   select s.Assignments into assigns
                                   from a in assigns
                                   select a into assign
                                   group assign by new { id = assign.Subject_Id, name = assign.Subject.Name } into grp
                                   orderby grp.Key.id
                                   select new
                                   {
                                       id = grp.Key.id,
                                       name = grp.Key.name,
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
                                                             id = sub.Id,
                                                             student = new { id = sub.Student_Id, name = sub.Student.User.FirstName + " " + sub.Student.User.LastName },
                                                             comment = sub.Comment,
                                                             date = sub.SubmitDate,
                                                             upload_id = sub.Upload_Id,
                                                             grading = new
                                                             {
                                                                 id = sub.Grading_Id,
                                                                 grade = sub.Grading_Id == null ? 0 : sub.Grading.Grade
                                                             }
                                                         }
                                       })
                                   }
                    };
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Submissions(int id)
        {
            var q = from a in db.Assignments
                    where a.Id == id
                    select a.Submissions into subs
                    from sub in subs
                    select new
                    {
                        id = sub.Id,
                        student = new { id = sub.Student_Id, name = sub.Student.User.FirstName + " " + sub.Student.User.LastName },
                        comment = sub.Comment,
                        date = sub.SubmitDate,
                        upload_id = sub.Upload_Id,
                        grading = new
                        {
                            id = sub.Grading_Id,
                            grade = sub.Grading_Id == null ? 0 : sub.Grading.Grade
                        }
                    };
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpStatusCodeResult Grade(GradingViewModel form)
        {
            var teacher = db.Teachers.Find(HttpContext.User.Identity.GetUserId<int>());
            var grading = db.Gradings.Add(new Grading
            {
                Grade = form.grade,
                Date = DateTime.Now,
                Feedback = form.feedback,
                Teacher_Id = teacher.Id
            });
            db.SaveChanges();
            var sub = db.Submissions.Find(form.sub_id);
            var name = sub.Student.User.FullName;
            sub.Grading_Id = grading.Id;
            db.SaveChanges();
            var msg = String.Format("Satte betyget ({0}) på '{1}'s inlämning", form.grade, name);
            return new HttpStatusCodeResult(200, msg);
        }

        public HttpStatusCodeResult Create(HttpPostedFileBase file)
        {
            var model = new JavaScriptSerializer().Deserialize(HttpContext.Request.Form["model"], typeof(AssignmentViewModel));
            return new HttpStatusCodeResult(500, "not implemented");
        }
    }
}