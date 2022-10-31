using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesiAdmin.Attributes
{
    [AttributeUsage(AttributeTargets.Class |
    AttributeTargets.Method,
    AllowMultiple = true)]
    public class RoleAuthAttribute : ActionFilterAttribute
    {
        private String _action = "Index";
        private String _controller = "News";
        private bool redirect = false;

        public string Roles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            User user;
            String[] roles;
            if (HttpContext.Current.Session["User"] != null)
            {
                user = (User)HttpContext.Current.Session["User"];
                if (Roles != null)
                {
                    roles = Roles.Split(',');
                    foreach (String item in roles)
                    {
                        if (item == user.Role.Name)
                        {
                            return;
                        }
                    }
                    redirect = true;
                }
                else
                {
                    if(user.Role_Id == 1)
                    {
                        return;
                    }
                    else
                    {
                        Uri myUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                        int id = int.Parse(myUri.Segments[3]);
                        if (user.Id == id)
                        {
                            return;
                        }
                        else
                        {
                            redirect = true;
                        }
                    }
                }
            }
            
            if (redirect)
            {
                string controllerName = _controller;
                if (string.IsNullOrEmpty(controllerName)) controllerName = actionContext.RequestContext.RouteData.GetRequiredString("controller");

                actionContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary()
                {
                    { "action", _action },
                    { "controller", controllerName }
                });
            }


        }
    }
}