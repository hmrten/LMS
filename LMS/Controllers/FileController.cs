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
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            //var file = new UploadedFile { OriginalFileName = upload.FileName, FileName = DateTime.Now.Ticks.ToString() };
            string fileName = Server.MapPath("~/Files/") + DateTime.Now.Ticks.ToString() + Path.GetRandomFileName();
            upload.SaveAs(fileName);

            return RedirectToAction("Index");
        }
    }
}