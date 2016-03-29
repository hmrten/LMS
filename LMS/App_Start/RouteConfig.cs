using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Teacher",
                url: "Teacher/{action}/{section}",
                defaults: new { controller = "Teacher", action = "Index", section = "Index" }
            );

			routes.MapRoute(
				name: "AngularMapping",
				url: "Teacher/{action}/{*catch-all}",
				defaults: new { controller = "Teacher", action = "Index" }
			);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
