using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesi.Controllers.Base
{
    public class BaseController : Controller
    {
      
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var a = filterContext.Controller;
            
            
                //filterContext.Result = new RedirectToRouteResult(
                //    new RouteValueDictionary {
                //{ "Controller", "YourControllerName" },
                //{ "Action", "YourAction" }
                //    });
            

            base.OnActionExecuting(filterContext);
        }
    }
}