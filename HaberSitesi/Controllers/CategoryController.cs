using DataAccess;
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
    public class CategoryController : Controller
    {
        private CategoryServices _categoryServices;
        public CategoryController()
        {
            _categoryServices = new CategoryServices();
        }

        // GET: Category
        [OutputCache(CacheProfile = "Cache10Min")]
        public ActionResult News(string categoryUrl, int? page)
        {
            int pageSize = 6;
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            ViewBag.Item = _categoryServices.GetNameByUrl(categoryUrl);
            try
            {
                if (categoryUrl != null)
                {
                    List<News> HaberListesi = _categoryServices.GetNewsByUrl(categoryUrl).OrderByDescending(m => m.PublishDate).ToList();
                    ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
                    if (!page.HasValue)
                    {
                        HaberListesi = HaberListesi.Take(pageSize).ToList();
                    }
                    else
                    {
                        int pageIndex = pageSize * page.Value;
                        HaberListesi = HaberListesi.Skip(pageIndex).Take(pageSize).ToList();
                    }
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_NewsInfinite", HaberListesi);
                    }
                    return View(HaberListesi);
                }
                else
                {
                    return View("~/Views/Error/Page404.cshtml");
                }
            }
            catch (Exception)
            {
                return View("~/Views/Error/Page404.cshtml");


                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }



        }
        [OutputCache(CacheProfile = "Cache10Min")]
        public ActionResult Albums(string categoryUrl, int? page)
        {
            int pageSize = 6;
            ViewBag.Item = "Galeri -" +_categoryServices.GetNameByUrl(categoryUrl);

            try
            {
                if (categoryUrl != null)
                {
                    List<Album> HaberListesi = _categoryServices.GetAlbumsByUrl(categoryUrl).OrderByDescending(m => m.PublishDate).ToList();
                    ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
                    if (!page.HasValue)
                    {
                        HaberListesi = HaberListesi.Take(pageSize).ToList();
                    }
                    else
                    {
                        int pageIndex = pageSize * page.Value;
                        HaberListesi = HaberListesi.Skip(pageIndex).Take(pageSize).ToList();
                    }
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_AlbumsInfinite", HaberListesi);
                    }
                    return View(HaberListesi);
                }
                else
                {
                    return View("~/Views/Error/Page404.cshtml");
                    // return View("~/Home/Index");
                }

            }
            catch (Exception)
            {
                return View("~/Views/Error/Page404.cshtml");
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

        }
        [OutputCache(CacheProfile = "Cache10Min")]
        public ActionResult Video(string categoryUrl, int? page)
        {
            int pageSize = 6;
            ViewBag.Item = "Video -" + _categoryServices.GetNameByUrl(categoryUrl);
            try
            {
                if (categoryUrl != null)
                {
                    List<Video> HaberListesi = _categoryServices.GetVideosByUrl(categoryUrl).OrderByDescending(m => m.PublishDate).ToList();
                    ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
                    if (!page.HasValue)
                    {
                        HaberListesi = HaberListesi.Take(pageSize).ToList();
                    }
                    else
                    {
                        int pageIndex = pageSize * page.Value;
                        HaberListesi = HaberListesi.Skip(pageIndex).Take(pageSize).ToList();
                    }
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_VideoInfinite", HaberListesi);
                    }
                    return View(HaberListesi);
                }
                else
                {
                     return View("~/Views/Error/Page404.cshtml");
                    //return View("~/Home/Index");
                }
            }
            catch (Exception)
            {
                return View("~/Views/Error/Page404.cshtml");
                //  return new httpstatuscoderesult(httpstatuscode.badrequest);

            }

        }


    }
}

