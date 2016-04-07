using LMS.DataAccess;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

namespace LMS.Controllers.API
{
	public class SubmissionController : Controller
	{
		private LMSContext db = new LMSContext();

		public AppUserManager UserManager
		{
			get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
		}


		public JsonResult MySubmissions()
		{
			var userId = HttpContext.User.Identity.GetUserId<int>();
			var student = db.Students.Where(s => s.User_Id == userId).SingleOrDefault();
			var res = db.Submissions.Where(s => s.Student_Id == student.Id)
				 .Select(t => new
					{
						grading_id = t.Grading_Id,
						submit_date = t.SubmitDate,
						comment = t.Comment,
						upload_id = t.Upload.Id,
						grading_grade = t.Grading_Id == null ? 0 : t.Grading.Grade,
						feedback = t.Grading_Id == null ? "" : t.Grading.Feedback,
						assignment = new
								{
									title = t.Assignment.Title,
									subject = t.Assignment.Subject.Name,
									description = t.Assignment.Description
								}
					});
			return Json(res, JsonRequestBehavior.AllowGet);
		}


		// LIST: Submission
		public JsonResult CurrentAssignments()
		{

			var userId = HttpContext.User.Identity.GetUserId<int>();
			var student = db.Students.Where(s => s.User_Id == userId).SingleOrDefault();
			var q = from g in db.Groups
					where g.Id == student.Group_Id
					select new
					{
						name = g.Name,
						subjects = from s in g.Subjects
								   select new
								   {
									   id = s.Id,
									   name = s.Name,
									   assignments = from a in s.Assignments
													 select new
													 {
														 title = a.Title,
														 description = a.Description,
														 date_start = a.DateStart,
														 date_end = a.DateEnd,
														 filepath = a.Upload.FilePath,
														 submissions = from sub in a.Submissions
																	   where sub.Assignment_Id == a.Id
																	   select new
																	   {
																		   sub.Grading_Id,
																		   sub.SubmitDate,
																		   sub.Comment,
																		   sub.Upload.FilePath
																	   }
													 }
								   }
					};
			return Json(q.SingleOrDefault(), JsonRequestBehavior.AllowGet);
		}

	}
}