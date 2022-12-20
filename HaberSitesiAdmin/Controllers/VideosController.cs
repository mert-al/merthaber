using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using DataAccess;
using FFmpeg.NET;
using HaberSitesiAdmin.Models;
using HaberSitesiAdmin.Services;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
//using Xabe.FFmpeg;
using NReco.VideoConverter;

namespace HaberSitesiAdmin.Controllers
{
    [ValidateInput(false)]
    [HandleError]
    public class VideosController : Controller
    {
        public static int progress { get; set; }
        VideoServices _videoServices;
        public VideosController()
        {
            _videoServices = new VideoServices();
        }

        // GET: Videos
        public ActionResult Index(PageDTO<Video> pageDTO)
        {
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            pageDTO.Index = pageDTO.Index == 0 ? 1 : pageDTO.Index;
            pageDTO.PageSize = pageDTO.PageSize == 0 ? 10 : pageDTO.PageSize;
            return View(_videoServices.GetPage(pageDTO));
        }

        public ActionResult Reviews(int? id)
        {
            var reviews = _videoServices.GetReviewsByVideoId(id.Value);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            ViewBag.apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            return View(reviews);
        }

        // GET: Videos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = _videoServices.Get(id.Value);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // GET: Videos/Create
        public ActionResult Create()
        {
            ViewBag.Categories = _videoServices.GetCategorySelectList();
            return View();
        }

        // POST: Videos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Img")] Video entity,
            HttpPostedFileBase file, HttpPostedFileBase canvas, HttpPostedFileBase videoFile, List<String> SelectedCategories,
            String publishDate, String MainSlider, String Sidebar, String SliderBottom, String BestWeekly, String BestWeeklySm, String NewsDetail, String OtherNews)


        {


            if (ModelState.IsValid && file != null && videoFile != null && SelectedCategories != null && !String.IsNullOrWhiteSpace(publishDate))
            {
                try
                {
                    entity.url = entity.Title.ToLower().Replace("ü", "u").Replace("ş", "s").Replace("ç", "c").Replace("ğ", "g").Replace("ö", "o").Replace("ı", "i") + "-";
                    entity.url = Regex.Replace(entity.url, @"[^0-9a-z]", "-").Replace("--", "-").Replace("--", "-").Replace("--", "-");
                    if (videoFile.ContentLength > 0)
                    {
                       
                        entity.EmbedUrl = _videoServices.SaveVideo(entity.url, videoFile);
                    }
                    
                    if (file.ContentLength > 0)
                        entity.Img = _videoServices.UpdateImage(entity.url, file);
                    


                    entity.MainSliderIMG = _videoServices.CreateCroppedImage(entity.url, MainSlider, "crop120x100"); ;
                    entity.SidebarIMG = _videoServices.CreateCroppedImage(entity.url, Sidebar, "crop120x100");
                    entity.SliderBottomIMG = _videoServices.CreateCroppedImage(entity.url, SliderBottom, "crop236x157");
                    entity.BestWeeklyIMG = _videoServices.CreateCroppedImage(entity.url, BestWeekly, "crop370x431");
                    entity.BestWeeklySmIMG = _videoServices.CreateCroppedImage(entity.url, BestWeeklySm, "crop270x174");
                    entity.DetailsIMG = _videoServices.CreateCroppedImage(entity.url, NewsDetail, "crop770x410");
                    entity.OtherIMG = _videoServices.CreateCroppedImage(entity.url, OtherNews, "crop370x344");
                    entity.PublishDate = DateTime.Parse(publishDate);
                    if (videoFile.ContentType == "video/mp4")
                    {
                        entity.ProcessingStatus = 2;
                    }
                    else
                    {
                        entity.ProcessingStatus = 0;
                    }
                   
                    _videoServices.Create(entity, SelectedCategories);
                    

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            if (videoFile == null)
            {
                ViewBag.videoUrlError = "Lütfen Video Yükleyiniz";
            }

            if (SelectedCategories == null)
            {
                ViewBag.categoryError = "Lütfen Kategori Seçiniz";
            }
            if (String.IsNullOrWhiteSpace(publishDate))
            {
                ViewBag.publishDateError = "Lütfen Yayın Tatihi Seçiniz";
            }
            ViewBag.Categories = _videoServices.GetCategorySelectList();
            return View(entity);
        }

        public async void ConvertVideo(string videoPath, string newVideoFilePath)
        {
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            //ffMpeg.ConvertProgress +=  (sender,args) {
            //    progress += 1;
            //    con
            //}
             ffMpeg.ConvertMedia(videoPath, newVideoFilePath, Format.mp4);


        }
        public int showProgress()
        {
            return progress;
        }
        //private void SaveUploadedFile(HttpPostedFileBase videoFile)
        //{
        //    Thread.Sleep(5000);
        //    var fileName = Path.GetFileName(videoFile.FileName);
        //    var path = Path.Combine(Server.MapPath("~/")+ "Storage\\Video\\Therad\\", fileName);
        //    videoFile.SaveAs(path);
        //}
        private string getVideoFileName(string fileName)
        {

            var fileNameArr = fileName.Split('.');
            return fileName.Replace(fileNameArr.LastOrDefault(), "mp4");
        }



        // GET: Videos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = _videoServices.Get(id.Value);
            if (video == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = _videoServices.GetSelectListByVideo(video);
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Img,EmbedUrl,Hit,PublishDate,CreatedDate,url,UpdatedDate,isActive,isDeleted,User_Id,MainSliderIMG,SidebarIMG,SliderBottomIMG,BestWeeklyIMG,BestWeeklySmIMG,DetailsIMG,OtherIMG")] Video entity,
            HttpPostedFileBase file, HttpPostedFileBase videoFile, List<String> SelectedCategories,
            String publishDate, String MainSlider, String Sidebar, String SliderBottom, String BestWeekly, String BestWeeklySm, String NewsDetail, String OtherNews)
        {
            if (ModelState.IsValid && videoFile != null && SelectedCategories != null && !String.IsNullOrWhiteSpace(publishDate))
            {
                try
                {
                    entity.url = entity.Title.ToLower().Replace("ü", "u").Replace("ş", "s").Replace("ç", "c").Replace("ğ", "g").Replace("ö", "o").Replace("ı", "i") + "-" + entity.Id;
                    entity.url = Regex.Replace(entity.url, @"[^0-9a-z]", "-").Replace("--", "-").Replace("--", "-").Replace("--", "-");
                
                    if (videoFile.ContentLength > 0)
                    {

                        entity.EmbedUrl = _videoServices.SaveVideo(entity.url, videoFile);
                        string newVideoFilePath = Server.MapPath("~/") + "Storage\\Video\\Videos\\" + getVideoFileName(videoFile.FileName);
                        var videoPath = Path.GetFullPath(Server.MapPath("~/") + entity.EmbedUrl);

                        if (videoFile.ContentType != "video/mp4")
                        {
                            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                            ffMpeg.ConvertMedia(videoPath, newVideoFilePath, Format.mp4);
                            entity.EmbedUrl = newVideoFilePath.Replace(AppContext.BaseDirectory, "\\");
                            _videoServices.DeleteVideo(videoPath);
                        }

                        using (var shell = ShellObject.FromParsingName(newVideoFilePath))
                        {
                            IShellProperty prop = shell.Properties.System.Media.Duration;
                            var t = (ulong)prop.ValueAsObject;
                            var durationtime = TimeSpan.FromTicks((long)t);
                            entity.VideoTime = durationtime.ToString();
                        }
                        entity.EmbedUrl = _videoServices.SaveVideo(entity.url, videoFile);
                    }
                    if (file.ContentLength > 0)
                    {
                        entity.Img = _videoServices.UpdateImage(entity.url, file);
                    }


                    entity.MainSliderIMG = _videoServices.UpdateCroppedImage(entity.url, MainSlider, entity.MainSliderIMG, "crop770x410slider");
                    entity.SidebarIMG = _videoServices.UpdateCroppedImage(entity.url, Sidebar, entity.SidebarIMG, "crop120x100");
                    entity.SliderBottomIMG = _videoServices.UpdateCroppedImage(entity.url, SliderBottom, entity.SliderBottomIMG, "crop236x157");
                    entity.BestWeeklyIMG = _videoServices.UpdateCroppedImage(entity.url, BestWeekly, entity.BestWeeklyIMG, "crop370x431");
                    entity.BestWeeklySmIMG = _videoServices.UpdateCroppedImage(entity.url, BestWeeklySm, entity.BestWeeklySmIMG, "crop270x174");
                    entity.DetailsIMG = _videoServices.UpdateCroppedImage(entity.url, NewsDetail, entity.DetailsIMG, "crop770x410");
                    entity.OtherIMG = _videoServices.UpdateCroppedImage(entity.url, OtherNews, entity.OtherIMG, "crop370x344");
                    entity.PublishDate = DateTime.Parse(publishDate);
                    _videoServices.Update(entity, SelectedCategories);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            }
            if (videoFile == null)
            {
                ViewBag.videoUrlError = "Lütfen Video Yükleyiniz";
            }
            if (file == null)
            {
                ViewBag.fileError = "Lütfen Video Görseli Yükleyiniz";
            }
            if (SelectedCategories == null)
            {
                ViewBag.categoryError = "Lütfen Kategori Seçiniz";
            }
            if (String.IsNullOrWhiteSpace(publishDate))
            {
                ViewBag.publishDateError = "Lütfen Yayın Tatihi Seçiniz";
            }
            ViewBag.Categories = _videoServices.GetCategorySelectList();
            return View(entity);
        }

        // GET: Videos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = _videoServices.Get(id.Value);
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
            _videoServices.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
