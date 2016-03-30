using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS
{
	public static class HtmlHelpers
	{
		public static MvcHtmlString IsActive(this HtmlHelper htmlHelper, string path, string activeClass, string inActiveClass = "")
		{
			var routeData = htmlHelper.ViewContext.RouteData;

			var routeController = routeData.Values["controller"].ToString();
			var routeAction = routeData.Values["action"].ToString();
			var routeSection = routeData.Values["section"].ToString();

			var routePath = string.Format("{0}/{1}/{2}", routeController, routeAction, routeSection);
			var returnActive = (path == routePath);

			return new MvcHtmlString(returnActive ? activeClass : inActiveClass);
		}

	}
}