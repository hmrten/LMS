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

        // LIST: Submission
		public JsonResult CurrentAssignments()
		{
			
			var user = UserManager.FindByName(HttpContext.User.Identity.Name);
			var student = db.Students.Where(s => s.User_Id == user.Id).SingleOrDefault();
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
														date_end =  a.DateEnd,
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
			//var q = from u in db.Users
			//	join r in db.Roles on u.Roles.FirstOrDefault().RoleId equals r.Id
			//	select new
			//	{
			//		id = u.Id,
			//		fname = u.FirstName,
			//		lname = u.LastName,
			//		email = u.Email,
			//		phone = u.PhoneNumber,
			//		uname = u.UserName,
			//		role_id = r.Id,
			//		role_name = r.Name
			//	}
			//		into usr
			//		group usr by new
			//		{
			//			id = usr.role_id,
			//			name = usr.role_name
			//		}
			//			into g
			//			orderby g.Key.id
			//			select new
			//			{
			//				role = g.Key.name,
			//				users = g.ToList()
			//			};
			//return Json(q, JsonRequestBehavior.AllowGet);
			return Json(q.SingleOrDefault(), JsonRequestBehavior.AllowGet);
		}

    }
}