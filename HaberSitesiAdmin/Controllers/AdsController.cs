using DataAccess;
using HaberSitesiAdmin.Models;
using HaberSitesiAdmin.Services;
using System;
using System.Net;
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
        public ActionResult Create([Bind(Include = "Id,Title,PrerolTitle,MidrollTitle,PostrollTitle,Midroll,Preroll,Postroll")] Ad reklam)

        {


            try
            {
                _adsServices.Create(reklam);
                _adsServices.GenerateXML(reklam);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
                //new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad reklam = _adsServices.Get(id.Value);
            if (reklam == null)
            {
                return HttpNotFound();
            }

            return View(reklam);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,PrerolTitle,MidrollTitle,PostrollTitle,Midroll,Preroll,Postroll")] Ad reklam)
        {

            try
            {
                _adsServices.GenerateXML(reklam);
                _adsServices.Update(reklam);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad video = _adsServices.Get(id.Value);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _adsServices.Delete(id);
            return RedirectToAction("Index");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var ad = _adsServices.Get(id.Value);
                if (ad == null)
                {
                    return HttpNotFound();
                }
                return View(ad);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
    }




}

