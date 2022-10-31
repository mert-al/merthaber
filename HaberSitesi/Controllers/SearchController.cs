using HaberSitesi.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesi.Controllers
{
    public class SearchController : Controller
    {
        NewsServices _newsServices;
        public SearchController()
        {
            _newsServices = new NewsServices();
        }
        // GET: Search
        public ActionResult Index(String query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            return View(_newsServices.SearchNews(query));
        }
    }
}