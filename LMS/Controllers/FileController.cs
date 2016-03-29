using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Models;

namespace LMS.Controllers
{
    public class FileController : Controller
    {
        public ViewResult Upload()
        {
            return View();
        }

        [HttpPost]
        public HttpStatusCodeResult Upload(string title, string description, HttpPostedFileBase file)
        {
            string fileName = Server.MapPath("~/Files/") + DateTime.Now.Ticks.ToString() + Path.GetRandomFileName();
            file.SaveAs(fileName);
            return new HttpStatusCodeResult(200, fileName + " uploaded successfully");
        }
    }
}