using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Net;

namespace LMS.Controllers
{
    public class AccountController : Controller
    {
        public AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }

        public AppSignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<AppSignInManager>(); }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string user, string pass)
        {
            var status = SignInManager.PasswordSignIn(user, pass, true, false);
            if (status == SignInStatus.Success)
            {
                RedirectToAction("Index");
            }
            ViewBag.Message = "faield to login";
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}