using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using HaberSitesiAdmin.Models;
using HaberSitesiAdmin.Services;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace HaberSitesiAdmin.Controllers
{
    //[AllowHtml]
    [ValidateInput(false)]
    public class NewsController : Controller
    {
        private NewsServices _newsServices;
        public NewsController()
        {
            _newsServices = new NewsServices();
        }


        //GET: News
        public ActionResult Index(PageDTO<News> pageDTO)
        {
            //using (var shell = ShellObject.FromParsingName(@"C:\Users\mertali.cetin\Source\Repos\mert-al\merthaber\HaberSitesiAdmin\Storage\Video\Videos\Küçük Çocuğun Sevimli Şikayeti.mp4"))
            //{
            //    IShellProperty prop = shell.Properties.System.Media.Duration;
            //    var t = (ulong)prop.ValueAsObject;
            //    var asd = TimeSpan.FromTicks((long)t);
            //}
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            pageDTO.Index = pageDTO.Index == 0 ? 1 : pageDTO.Index;
            pageDTO.PageSize = pageDTO.PageSize == 0 ? 10 : pageDTO.PageSize;
            return View(_newsServices.GetPage(pageDTO));
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var news = _newsServices.Get(id.Value);
                if (news == null)
                {
                    return HttpNotFound();
                }
                return View(news);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }


        public ActionResult Reviews(int? id)
        {
            var reviews = _newsServices.GetReviewsByNewsId(id.Value);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            return View(reviews);
        }


        // GET: News/Create
        public ActionResult Create()
        {
            ViewBag.Categories = _newsServices.GetCategorySelectList();
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Content,Img,PublishDate,Hit,isActive,TrendingNow,isDeleted,CreatedDate,UpdatedDate")] News entity, HttpPostedFileBase file, List<String> SelectedCategories, String publishDate, String MainSlider, String Sidebar, String SliderBottom, String BestWeekly, String BestWeeklySm, String NewsDetail, String OtherNews)
        {

            if (ModelState.IsValid && file != null && SelectedCategories != null && !String.IsNullOrWhiteSpace(publishDate))
            {
                try
                {
                    entity.url = entity.Title.ToLower().Replace("ü", "u").Replace("ş", "s").Replace("ç", "c").Replace("ğ", "g").Replace("ö", "o").Replace("ı", "i");
                    entity.url = Regex.Replace(entity.url, @"[^0-9a-z]", "-").Replace("--", "-").Replace("--", "-").Replace("--", "-");
                    if (file.ContentLength > 0)
                    {
                        entity.Img = _newsServices.UpdateImage(entity.url, file);
                    }
                    entity.MainSliderIMG = _newsServices.CreateCroppedImage(entity.url, MainSlider, "crop770x410slider");
                    entity.SidebarIMG = _newsServices.CreateCroppedImage(entity.url, Sidebar, "crop120x100");
                    entity.SliderBottomIMG = _newsServices.CreateCroppedImage(entity.url, SliderBottom, "crop236x157");
                    entity.BestWeeklyIMG = _newsServices.CreateCroppedImage(entity.url, BestWeekly, "crop370x431");
                    entity.BestWeeklySmIMG = _newsServices.CreateCroppedImage(entity.url, BestWeeklySm, "crop270x174");
                    entity.DetailsIMG = _newsServices.CreateCroppedImage(entity.url, NewsDetail, "crop770x410");
                    entity.OtherIMG = _newsServices.CreateCroppedImage(entity.url, OtherNews, "crop370x344");
                    entity.PublishDate = DateTime.Parse(publishDate);
                    _newsServices.Create(entity, SelectedCategories);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            if (file == null)
            {
                ViewBag.fileError = "Lütfen Haber Görseli Yükleyiniz";
            }
            if (SelectedCategories == null)
            {
                ViewBag.categoryError = "Lütfen Kategori Seçiniz";
            }
            if (String.IsNullOrWhiteSpace(publishDate))
            {
                ViewBag.publishDateError = "Lütfen Yayın Tatihi Seçiniz";
            }
            ViewBag.Categories = _newsServices.GetCategorySelectList();
            return View(entity);


        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = _newsServices.Get(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = _newsServices.GetSelectListByNews(news);
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "Id,Title,Description,Content,Img,PublishDate,Hit,isActive,TrendingNow,isDeleted,CreatedDate,UpdatedDate,User_Id,MainSliderIMG,SidebarIMG,SliderBottomIMG,BestWeeklyIMG,BestWeeklySmIMG,DetailsIMG,OtherIMG")] News entity,
            HttpPostedFileBase file, List<String> SelectedCategories,
            String publishDate, String MainSlider, String Sidebar, String SliderBottom, String BestWeekly, String BestWeeklySm, String NewsDetail, String OtherNews)
        {
            if (ModelState.IsValid && SelectedCategories != null && !String.IsNullOrWhiteSpace(publishDate))
            {
                try
                {
                    entity.url = entity.Title.ToLower().Replace("ü", "u").Replace("ş", "s").Replace("ç", "c").Replace("ğ", "g").Replace("ö", "o").Replace("ı", "i") + "-" + entity.Id;
                    entity.url = Regex.Replace(entity.url, @"[^0-9a-z]", "-").Replace("--", "-").Replace("--", "-").Replace("--", "-"); ;
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            entity.Img = _newsServices.UpdateImage(entity.url, file);
                        }
                    }

                    entity.MainSliderIMG = _newsServices.UpdateCroppedImage(entity.url, MainSlider, entity.MainSliderIMG, "crop770x410slider");
                    entity.SidebarIMG = _newsServices.UpdateCroppedImage(entity.url, Sidebar, entity.SidebarIMG, "crop120x100");
                    entity.SliderBottomIMG = _newsServices.UpdateCroppedImage(entity.url, SliderBottom, entity.SliderBottomIMG, "crop236x157");
                    entity.BestWeeklyIMG = _newsServices.UpdateCroppedImage(entity.url, BestWeekly, entity.BestWeeklyIMG, "crop370x431");
                    entity.BestWeeklySmIMG = _newsServices.UpdateCroppedImage(entity.url, BestWeeklySm, entity.BestWeeklySmIMG, "crop270x174");
                    entity.DetailsIMG = _newsServices.UpdateCroppedImage(entity.url, NewsDetail, entity.DetailsIMG, "crop770x410");
                    entity.OtherIMG = _newsServices.UpdateCroppedImage(entity.url, OtherNews, entity.OtherIMG, "crop370x344");
                    entity.PublishDate = DateTime.Parse(publishDate);
                    _newsServices.Update(entity, SelectedCategories);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            }
            if (SelectedCategories == null)
            {
                ViewBag.categoryError = "Lütfen Kategori Seçiniz";
            }
            if (String.IsNullOrWhiteSpace(publishDate))
            {
                ViewBag.publishDateError = "Lütfen Yayın Tatihi Seçiniz";
            }
            ViewBag.Categories = _newsServices.GetCategorySelectList();
            return View(entity);
        }


        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = _newsServices.Get(id.Value);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _newsServices.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

