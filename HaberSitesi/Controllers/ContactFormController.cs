using DataAccess;
using HaberSitesi.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesi.Controllers
{
    public class ContactFormController : Controller
    {
        ContactFormServices _contactFormServices;
        public ContactFormController()
        {
            _contactFormServices = new ContactFormServices();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index([Bind(Include = "Id,Name,Lastname,EMail,Phone,Message,Subject,isActive,isDeleted,CreatedDate,UpdatedDate")] ContactForm forms)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = Request["g-recaptcha-response"];
                    const string secret = "6LevuTohAAAAAHnrWHilHn5IMMIBWn3P5BQOJNd6";

                    var client = new WebClient();
                    var reply =
                        client.DownloadString(
                            string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

                    var captchaResponse = JsonConvert.DeserializeObject<HaberSitesi.Models.CaptchaResponse>(reply);

                    if (!captchaResponse.Success)
                    {
                        ViewBag.Error = "Recaptcha doğrulayın!";
                        return View();
                    }
                    else
                    {
                        _contactFormServices.Create(forms);
                        ViewBag.Message = "Form Basariyla Gonderildi";
                    }
                }
                return View();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}