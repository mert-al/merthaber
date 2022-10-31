using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesi.Controllers
{
    public class ErrorController: Controller
    {
        [HandleError]
        public ActionResult Page404()
        {
            Response.StatusCode = 404;  
            return View("Page404");
        }
       

    }
}