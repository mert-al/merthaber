using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using HaberSitesi.Services;

namespace HaberSitesi.Controllers
{
    public class ReviewsController : Controller
    {
        ReviewServices _reviewServices;
        public ReviewsController()
        {
            _reviewServices = new ReviewServices();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,EMail,Title,Message,CreatedDate,UpdatedDate,isActive,isDeleted,News_Id,News,Album_Id,Album,Video_Id,Video")] Review reviews, int? id)
        {
            string returnURL = Request.UrlReferrer.ToString();
           
           
            if (ModelState.IsValid)
            {
                _reviewServices.Create(reviews);
            }
            ViewBag.ResponseMessage = "Yorumunuz Başarılı Bir Şekilde Kaydedilmiştir";
            return Redirect(returnURL);
        }
    }
}
