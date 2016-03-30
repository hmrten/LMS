using LMS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class GroupController : Controller
    {
        private LMSContext db = new LMSContext();

        public JsonResult List()
        {
            var q = db.Groups.Select(g =>
                new
                {
                    id = g.Id,
                    name = g.Name
                });
            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}