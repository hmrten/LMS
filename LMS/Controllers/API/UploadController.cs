using LMS.DataAccess;
using LMS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers.API
{
    public class UploadController : Controller
    {
        private LMSContext db = new LMSContext();

        [HttpGet]
        public FileResult Get(int id)
        {
            var upload = db.Uploads.Find(id);
            return File(upload.FilePath, "application/octet-stream", Path.GetFileName(upload.FilePath));
        }
    }
}