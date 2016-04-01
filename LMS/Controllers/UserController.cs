using LMS.DataAccess;
using LMS.Models;
using LMS.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class UserController : Controller
    {
        private LMSContext db = new LMSContext();
        private AppUserManager userManager;

        public UserController()
        {
            userManager = AppUserManager.Create(new IntUserStore(db));
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

				var result = userManager.ChangePassword(user.id, user.oldpassword, user.password1);

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
            var role = userManager.GetRoles(id).First();
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
            var q = (from u in db.Users
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
                     }).GroupBy(g => g.role_name).Select(x => new
                     {
                         role = x.Key,
                         users = x.ToList()
                     });
            return Json(q, JsonRequestBehavior.AllowGet);
        }


    }
}