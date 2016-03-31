using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Models;

namespace LMS.Controllers
{
    public class DirTree
    {
        public string name;
        public List<DirTree> children;
    }

    public class FileManagerController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

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

        private void WalkDirTree(DirectoryInfo root, ref DirTree tree)
        {
            tree.name = root.Name;
            var subDirs = root.GetDirectories();
            if (subDirs.Count() > 0)
            {
                tree.children = new List<DirTree>(subDirs.Count());
                foreach (var dir in subDirs)
                {
                    var newTree = new DirTree { name = dir.Name, children = null };
                    tree.children.Add(newTree);
                    WalkDirTree(dir, ref newTree);
                }
            }
        }

        public JsonResult List()
        {
            var filesPath = Server.MapPath("~/Files");
            var dirs = Directory.EnumerateDirectories(filesPath, "*", SearchOption.AllDirectories)
                .Select(d => d.Replace(filesPath, ""));

            var tree = new DirTree();
            WalkDirTree(new DirectoryInfo(filesPath), ref tree);

            return Json(tree, JsonRequestBehavior.AllowGet);
        }
    }
}