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
using System.Security.Claims;

namespace LMS.Controllers
{
    public class UserController : Controller
    {
        private LMSContext db = new LMSContext();
		
        public AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }

        public AppSignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<AppSignInManager>(); }
        }

		[HttpPost]
		public HttpStatusCodeResult Create(UserViewModel user)
		{
			if (db.Users.Any(u => u.UserName == user.uname))
			{
				return new HttpStatusCodeResult(417, "Användarnamnet finns redan, välj ett nytt");
			}
			
			if (user.password1 != user.password2)
			{
				return new HttpStatusCodeResult(417, "Lösenorden matchar inte");
			}
			
			if (!ModelState.IsValid)
			{
				return new HttpStatusCodeResult(417, "Du glömde fylla i obligatoriska fält markerade med *");
			}

			var userEnt = new User { FirstName = user.fname, LastName = user.lname, Email = user.email, UserName = user.uname };


			var res = UserManager.Create(userEnt, user.password1);
			if (!res.Succeeded)
			{
				throw new Exception(res.Errors.Aggregate("", (a, b) => a + b + "\n"));
			}

			UserManager.AddToRole(userEnt.Id, user.role_name);

			switch (user.role_name)
			{
				case "teacher":
					db.Teachers.Add(new Teacher { User_Id = userEnt.Id });
					break;
				case "student":
					db.Students.Add(new Student { User_Id = userEnt.Id });
					break;
				default:
					break;
			}

			db.SaveChanges();

			var message = string.Format("En {0} med användarnamn: {1} skapades", user.role_name, userEnt.UserName);
			return new HttpStatusCodeResult(200, message);
		}

		[HttpPost]
		public HttpStatusCodeResult Update(UserViewModel user)
		{
			var userEnt = db.Users.Where(u => u.Id == user.id).SingleOrDefault();

			userEnt.FirstName = user.fname;
			userEnt.LastName = user.lname;
			userEnt.Email = user.email;
			userEnt.PhoneNumber = user.phone;

			if(user.oldpassword != null){
				if (user.password1 != user.password2)
				{
					return new HttpStatusCodeResult(417, "Lösenorden matchar inte");
				}

				var result = UserManager.ChangePassword(user.id, user.oldpassword, user.password1);

				if (result != IdentityResult.Success)
				{
					return new HttpStatusCodeResult(403, "Nuvarande lösenordet är fel");
				}
			}

			db.SaveChanges();
			return new HttpStatusCodeResult(200, "En användare med id: " + userEnt.Id.ToString() + "uppdaterades");
		}

        public JsonResult GetRoles()
        {
            var q = db.Roles.Select(r =>
                new
                {
                    id = r.Id,
                    name = r.Name
                });

            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUser(int id)
        {
            var role = UserManager.GetRoles(id).First();
            var user = db.Users.Where(s => s.Id == id).Select(s =>
                new
                {
                    id = s.Id,
                    fname = s.FirstName,
                    lname = s.LastName,
                    email = s.Email,
                    phone = s.PhoneNumber,
					uname = s.UserName,
                    role_id = s.Roles.FirstOrDefault().RoleId,
                    role_name = role
                }).SingleOrDefault();

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public JsonResult List()
        {
            var q = from u in db.Users
                    join r in db.Roles on u.Roles.FirstOrDefault().RoleId equals r.Id
                    select new
                    {
                        id = u.Id,
                        fname = u.FirstName,
                        lname = u.LastName,
                        email = u.Email,
                        phone = u.PhoneNumber,
                        uname = u.UserName,
                        role_id = r.Id,
                        role_name = r.Name
                    }
                        into usr
                        group usr by new
                        {
                            id = usr.role_id,
                            name = usr.role_name
                        }
                            into g
                            orderby g.Key.id
                            select new
                            {
                                role = g.Key.name,
                                users = g.ToList()
                            };
            return Json(q, JsonRequestBehavior.AllowGet);
        }
		
        [AllowAnonymous]
        [HttpPost]
        public RedirectToRouteResult Login(string user, string pass)
        {
            var status = SignInManager.PasswordSignIn(user, pass, true, false);
            if (status == SignInStatus.Success)
            {
                var userEnt = UserManager.FindByName(user);
                var role = db.Roles.Find(userEnt.Roles.First().RoleId).Name;
                Session["role"] = role;
                return RedirectToAction("Index", role);
            }
            return RedirectToAction("Login", "Home", new { msg = "failed to login" });
        }

        public RedirectToRouteResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            Session["role"] = "guest";
            return RedirectToAction("Index", "Home");
        }

		public JsonResult StudentIdentity()
		{
			var user = UserManager.FindByName(User.Identity.Name);
			var student = db.Students.Where(s => s.User_Id == user.Id)
				.Select(t => new
                {
                    id = t.User_Id
                }).SingleOrDefault();

			return Json(student, JsonRequestBehavior.AllowGet);
		}

		public JsonResult TeacherIdentity()
		{
			var user = UserManager.FindByName(User.Identity.Name);
			var teacher = db.Teachers.Where(s => s.User_Id == user.Id)
				.Select(t => new
					{
						id = t.User_Id
					}).SingleOrDefault();
			return Json(teacher, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public HttpStatusCodeResult UpdatePassword(UserViewModel user)
		{
			if (user.oldpassword != null)
			{
				if (user.password1 != user.password2)
				{
					return new HttpStatusCodeResult(417, "Lösenorden matchar inte");
				}

				var result = UserManager.ChangePassword(user.id, user.oldpassword, user.password1);

				if (result != IdentityResult.Success)
				{
					return new HttpStatusCodeResult(403, "Nuvarande lösenordet är fel");
				}
			}

			db.SaveChanges();
			return new HttpStatusCodeResult(200, "En användare med id: " + user.id.ToString() + "uppdaterades");
		}
    }
}