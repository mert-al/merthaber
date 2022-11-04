using DataAccess;
using HaberSitesiAdmin.Models;
using HaberSitesiAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesiAdmin.Controllers
{
    public class AdsController : Controller
    {
        AdsServices _adsServices;
        public AdsController()
        {
            _adsServices = new AdsServices();
        }
        // GET: Ads
        public ActionResult Index(PageDTO<Ad> pageDTO)
        {
            //ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            pageDTO.Index = pageDTO.Index == 0 ? 1 : pageDTO.Index;
            pageDTO.PageSize = pageDTO.PageSize == 0 ? 10 : pageDTO.PageSize;
            return View(_adsServices.GetPage(pageDTO));
        }
        //public ActionResult Create(Ad reklam)
        //{
        //    _adsServices.Create(reklam);
        //    _adsServices.GenerateXML(reklam);
        //    return RedirectToAction("Index");
        //}
        public ActionResult Create()
        {
            //ViewBag.Categories = _videoServices.GetCategorySelectList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Img,EmbedUrl")] Ad reklam)

        {
           
                try
                {

                
                    //if (videoFile.ContentLength > 0)
                    //{

                    //    return _adsServices.GenerateXML(reklam);
                    //}


                    //reklam.PublishDate = DateTime.Parse(publishDate);
                    _adsServices.Create(reklam);
                    _adsServices.GenerateXML(reklam);





                    return RedirectToAction("Index");
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }        }





    }
}
