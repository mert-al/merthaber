using DataAccess;
using HaberSitesiAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HaberSitesiAdmin.Models;


namespace HaberSitesiAdmin.Controllers
{
    [AllowAnonymous]
    [HandleError]
    public class SecurityController : Controller
    {
        private UserServices _userServices;
        public SecurityController()
        {
            _userServices = new UserServices();
        }
        // GET: Security
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            LoginResult result = _userServices.Login(user);
            if (result.Status)
            {
                return RedirectToAction("Index", "News");
            }
            else
            {
                ViewBag.Message = result.Message;
                return View();
            }
        }
        public ActionResult Logout()
        {
            _userServices.Logout();
            return RedirectToAction("Login");
        }
    }
}