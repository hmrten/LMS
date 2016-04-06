using LMS.DataAccess;
using LMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LMS.Controllers.API
{
    public class AssignmentController : Controller
    {
        private LMSContext db = new LMSContext();

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
                                                             student = new { id = sub.Student_Id, name = sub.Student.User.FirstName + " " + sub.Student.User.LastName },
                                                             comment = sub.Comment,
                                                             date = sub.SubmitDate,
                                                             filepath = sub.Upload.FilePath,
                                                             grading_id = sub.Grading_Id
                                                         }
                                       })
                                   }
                    };
                    //select g.Subjects into gs
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        public HttpStatusCodeResult Create(HttpPostedFileBase file)
        {
            var model = new JavaScriptSerializer().Deserialize(HttpContext.Request.Form["model"], typeof(AssignmentViewModel));
            return new HttpStatusCodeResult(500, "not implemented");
        }
    }
}