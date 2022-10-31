using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HaberSitesi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            var httpContext = ((MvcApplication)sender).Context;
            var exception = Server.GetLastError();
            if (exception == null) return;

            //if(Context.Server.GetLastError() as HttpException).GetHttpCode() == System.Net.HttpStatusCode.NotFound )
            var httpStatusCode =(Server.GetLastError() as HttpException).GetHttpCode();
            if (httpStatusCode == (int)System.Net.HttpStatusCode.NotFound || httpStatusCode == (int)System.Net.HttpStatusCode.BadRequest)
            {
                httpContext.Response.Redirect("/hata/Page404");
            }
            //int httpCode = (int)System.Net.HttpStatusCode.NotFound;

            RouteData rd = new RouteData();
            //httpContext.Response.Redirect("/hata/Page404");

            rd.Values.Add("controller", "Error");
            rd.Values.Add("action", "Page404");


            if (exc is HttpUnhandledException)
            {
                // Pass the error on to the error page.
                Server.Transfer("ErrorPage.aspx?handler=Application_Error%20-%20Global.asax", true);
            }
        }

        private void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            // Handle specific exception.
            if (exc is HttpUnhandledException)
            {
            }
            // Clear the error from the server.
            Server.ClearError();
        }



    }
}
