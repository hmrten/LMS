using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		public void Application_PreRequestHandlerExecute(Object o, EventArgs e)
		{
			var app = (HttpApplication)o;
			var ctx = app.Context;
			if (ctx != null && ctx.Session != null)
			{
                if (ctx.Request.IsAuthenticated)
                {
                    ctx.Session["role"] = UserHelpers.GetRoles().First();
                }
                else
				{
					ctx.Session["role"] = "guest";
				}
			}
		}
    }
}
