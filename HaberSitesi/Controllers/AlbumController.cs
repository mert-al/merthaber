using DataAccess;
using HaberSitesi.Models;
using HaberSitesi.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesi.Controllers
{
    public class AlbumController : Controller
    {
        private AlbumServices _albumServices;
        public AlbumController()
        {
            _albumServices = new AlbumServices();
        }

        // GET: Albums
        [OutputCache(CacheProfile = "Cache10Min")]
        public ActionResult Index()
        {
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            return View(_albumServices.GetdtoHomePage());
        }
        public ActionResult Details(String categoryUrl, String albumUrl,int? imageId)
        {
            if (String.IsNullOrWhiteSpace(categoryUrl) || String.IsNullOrWhiteSpace(albumUrl))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                string returnURL = "";
                if (Request.UrlReferrer != null) // son url e geri döndürür
                {
                    returnURL = Request.UrlReferrer.ToString();
                }

                ViewBag.imageIndex = imageId;
                DetailsDto<Album> dtoAlbumsDetails = _albumServices.GetAlbumsDetails(categoryUrl, albumUrl);
                if (dtoAlbumsDetails.Item == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
                return View(dtoAlbumsDetails);

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}