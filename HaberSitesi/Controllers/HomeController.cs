using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using DataAccess;
using HaberSitesi.Models;
using HaberSitesi.Services;

namespace HaberSitesi.Controllers
{
    public class HomeController : Controller
    {
        private NewsServices _newsServices;
        public HomeController()
        {
            _newsServices = new NewsServices();
        }
        public ActionResult Index()
        {
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            return View(_newsServices.GetdtoHomePage());
        }


    }
} 