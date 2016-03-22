using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            string ts = DateTime.Now.Ticks.ToString();
            return RedirectToAction("Index");
        }
    }
}