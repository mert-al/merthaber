using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using HaberSitesiAdmin.Models;
using HaberSitesiAdmin.Services;

namespace HaberSitesiAdmin.Controllers
{
    [HandleError]
    public class AlbumsController : Controller
    {
        private AlbumServices _albumServices;
        public AlbumsController()
        {
            _albumServices = new AlbumServices();
        }

        // GET: Albums
        public ActionResult Index(PageDTO<Album> pageDTO)
        {
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            pageDTO.Index = pageDTO.Index == 0 ? 1 : pageDTO.Index;
            pageDTO.PageSize = pageDTO.PageSize == 0 ? 10 : pageDTO.PageSize;
            return View(_albumServices.GetPage(pageDTO));
        }

        public ActionResult Reviews(int? id)
        {
            var reviews = _albumServices.GetReviewsByAlbumId(id.Value);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            return View(reviews);
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = _albumServices.Get(id.Value);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            ViewBag.Categories = _albumServices.GetCategorySelectList();
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,Description,isActive,isDeleted,CreatedDate,UpdatedDate,ImgDescription")] Album album, List<AlbumIMGs> albumIMGs, HttpPostedFileBase file, List<String> SelectedCategories, String publishDate,
            String MainSlider, String Sidebar, String SliderBottom, String BestWeekly, String BestWeeklySm, String NewsDetail, String OtherIMGs)
        {
            if (ModelState.IsValid && file != null && SelectedCategories != null && !String.IsNullOrWhiteSpace(publishDate))
            {
                try
                {
                    album.url = album.Title.ToLower().Replace("ü", "u").Replace("ş", "s").Replace("ç", "c").Replace("ğ", "g").Replace("ö", "o").Replace("ı", "i") + "-"; ;
                    album.url = Regex.Replace(album.url, @"[^0-9a-z]", "-").Replace("--", "-").Replace("--", "-").Replace("--", "-");
                    if (file.ContentLength > 0)
                    {
                        album.Img = _albumServices.UpdateImage((album.url + "-" + 0), file);
                    }
                    album.ImgDescription = album.ImgDescription == null ? "" : album.ImgDescription;
                    if (albumIMGs != null)
                    {
                        if (albumIMGs.Count > 0)
                        {
                            for (int i = 0; i < albumIMGs.Count(); i++)
                            {
                                if (albumIMGs[i].File != null)
                                {
                                    Random number = new Random();
                                    album.AlbumIMGs.Add(new AlbumIMG
                                    {
                                        Title = albumIMGs[i].Title == null ? "" : albumIMGs[i].Title,
                                        Description = albumIMGs[i].Description == null ? "" : albumIMGs[i].Description,
                                        IMG = _albumServices.UpdateImage((album.url + "-" + number.Next(0, 100000) + "-" + (i + 1)), albumIMGs[i].File)
                                    });
                                }

                            }
                        }
                    }
                    album.MainSliderIMG = _albumServices.CreateCroppedImage(album.url, MainSlider, "crop770x410slider");
                    album.SidebarIMG = _albumServices.CreateCroppedImage(album.url, Sidebar, "crop120x100");
                    album.SliderBottomIMG = _albumServices.CreateCroppedImage(album.url, SliderBottom, "crop236x157");
                    album.BestWeeklyIMG = _albumServices.CreateCroppedImage(album.url, BestWeekly, "crop370x431");
                    album.BestWeeklySmIMG = _albumServices.CreateCroppedImage(album.url, BestWeeklySm, "crop270x174");
                    album.DetailsIMG = _albumServices.CreateCroppedImage(album.url, NewsDetail, "crop770x410");
                    album.OtherIMG = _albumServices.CreateCroppedImage(album.url, OtherIMGs, "crop370x344");
                    album.PublishDate = DateTime.Parse(publishDate);
                    _albumServices.Create(album, SelectedCategories);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
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

            ViewBag.Categories = _albumServices.GetCategorySelectList();
            return View(album);
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = _albumServices.Get(id.Value);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = _albumServices.GetSelectListByAlbum(album);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Album album, List<AlbumIMGs> newAlbumIMGs, HttpPostedFileBase file, List<String> SelectedCategories, String publishDate,
            String MainSlider, String Sidebar, String SliderBottom, String BestWeekly, String BestWeeklySm, String NewsDetail, String OtherIMGs, List<UpdatedImages> updatedImages)
        {
            //if (ModelState.IsValid)
            //{
            if (!String.IsNullOrWhiteSpace(album.Title) && SelectedCategories != null && !String.IsNullOrWhiteSpace(publishDate))
            {
                try
                {
                    album.url = album.Title.ToLower().Replace("ü", "u").Replace("ş", "s").Replace("ç", "c").Replace("ğ", "g").Replace("ö", "o").Replace("ı", "i") + "-";
                    album.url = Regex.Replace(album.url, @"[^0-9a-z]", "-").Replace("--", "-").Replace("--", "-").Replace("--", "-");
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            album.Img = _albumServices.UpdateImage((album.url + "-" + 0), file);
                        }
                    }
                    album.ImgDescription = album.ImgDescription == null ? "" : album.ImgDescription;
                    List<AlbumIMG> newImages = new List<AlbumIMG>();
                    if (updatedImages != null)
                    {
                        if (updatedImages.Count() > 0)
                        {
                            _albumServices.UpdateAlbumImages(updatedImages);
                        }
                    }
                    if (newAlbumIMGs != null)
                    {
                        if (newAlbumIMGs.Count > 0)
                        {
                            for (int i = 0; i < newAlbumIMGs.Count(); i++)
                            {
                                if (newAlbumIMGs[i] != null)
                                {
                                    Random number = new Random();
                                    album.AlbumIMGs.Add(new AlbumIMG
                                    {
                                        Title = newAlbumIMGs[i].Title,
                                        Description = newAlbumIMGs[i].Description,
                                        IMG = _albumServices.UpdateImage((album.url + "-" + number.Next(0, 100000) + "-" + (i + 1)), newAlbumIMGs[i].File),
                                        CreatedDate = DateTime.UtcNow.AddHours(3),
                                        UpdatedDate = DateTime.UtcNow.AddHours(3)
                                    });
                                }
                            }
                        }
                    }

                    album.MainSliderIMG = _albumServices.UpdateCroppedImage(album.url, MainSlider, album.MainSliderIMG, "crop770x410slider");
                    album.SidebarIMG = _albumServices.UpdateCroppedImage(album.url, Sidebar, album.SidebarIMG, "crop120x100");
                    album.SliderBottomIMG = _albumServices.UpdateCroppedImage(album.url, SliderBottom, album.SliderBottomIMG, "crop236x157");
                    album.BestWeeklyIMG = _albumServices.UpdateCroppedImage(album.url, BestWeekly, album.BestWeeklyIMG, "crop370x431");
                    album.BestWeeklySmIMG = _albumServices.UpdateCroppedImage(album.url, BestWeeklySm, album.BestWeeklySmIMG, "crop270x174");
                    album.DetailsIMG = _albumServices.UpdateCroppedImage(album.url, NewsDetail, album.DetailsIMG, "crop770x410");
                    album.OtherIMG = _albumServices.UpdateCroppedImage(album.url, OtherIMGs, album.OtherIMG, "crop370x344");
                    album.PublishDate = DateTime.Parse(publishDate);
                    _albumServices.Update(album, SelectedCategories);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (String.IsNullOrWhiteSpace(album.Title))
            {
                ViewBag.titleError = "Lütfen Haber Başlıgı Giriniz";
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

            ViewBag.Categories = _albumServices.GetCategorySelectList();
            return View(album);

            //}
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Name", album.UserId);
            //return View(album);
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = _albumServices.Get(id.Value);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _albumServices.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
