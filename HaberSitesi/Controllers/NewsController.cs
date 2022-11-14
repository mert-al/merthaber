using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using HaberSitesi.Models;
using HaberSitesi.Services;

namespace HaberSitesi.Controllers
{
    public class NewsController : Controller
    {
        private NewsServices _newsServices;
        public NewsController()
        {
            _newsServices = new NewsServices();
        }
      

        public ActionResult Details(String categoryUrl, String newsUrl,string[] news)
        {
            if (Request.IsAjaxRequest())
            {
                
                try
                {
                    //string[] Ar;
                    if (news == null)
                    {
                        news = new string[] {newsUrl};
                    }
                    //return PartialView("~/Views/Shared/Component/_DetailsInfinite.cshtml", _newsServices.GetInfinite(newsUrl, news));
                    return PartialView("_DetailInfinite", _newsServices.GetInfinite(newsUrl, news));
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(categoryUrl) || String.IsNullOrWhiteSpace(newsUrl))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                try
                {
                    DetailsDto<News> dtoNewsDetails = _newsServices.GetNewsDetails(categoryUrl, newsUrl);
                    ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
                    if (dtoNewsDetails.Item == null)
                    {
                        return View("~/Views/Error/Page404.cshtml");
                    }
                    ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
                    return View(dtoNewsDetails);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
