using DataAccess;
using HaberSitesi.Models;
using HaberSitesi.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace HaberSitesi.Controllers
{

    public class VideoController : Controller
    {
        private VideoServices _videoServices;

        public VideoController()
        {
            _videoServices = new VideoServices();
        }

        // GET: Video
        public ActionResult Index()
        {

            //GenerateXML();
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            return View(_videoServices.GetdtoHomePage());
        }
        public ActionResult Details(String categoryUrl, String videoUrl)
        {
            if (String.IsNullOrWhiteSpace(categoryUrl) || String.IsNullOrWhiteSpace(videoUrl))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                DetailsDto<Video> dtoVideoDetails = _videoServices.GetVideosDetails(categoryUrl, videoUrl);
                if (dtoVideoDetails.Item == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
                //dtoVideoDetails.Ads = Ad.services(getAdByReklamType )
                return View(dtoVideoDetails);

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}





