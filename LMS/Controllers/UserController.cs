using LMS.DataAccess;
using LMS.Models;
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