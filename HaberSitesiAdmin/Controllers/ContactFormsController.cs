using System.Net;
using System.Web.Mvc;
using DataAccess;
using HaberSitesiAdmin.Models;
using HaberSitesiAdmin.Services;

namespace HaberSitesiAdmin.Controllers
{
    public class ContactFormsController : Controller
    {
        private ContactFormServices _contactFormServices;
        public ContactFormsController()
        {
            _contactFormServices = new ContactFormServices();
        }

        // GET: ContactForms
        public ActionResult Index(PageDTO<ContactForm> pageDTO)
        {
            pageDTO.Index = pageDTO.Index == 0 ? 1 : pageDTO.Index;
            pageDTO.PageSize = pageDTO.PageSize == 0 ? 10 : pageDTO.PageSize;
            return View(_contactFormServices.GetPage(pageDTO));
        }

        // GET: ContactForms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactForm contactForm = _contactFormServices.GetDetails(id.Value);
            if (contactForm == null)
            {
                return HttpNotFound();
            }
            return View(contactForm);
        }

        // GET: ContactForms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactForm contactForm = _contactFormServices.Get(id.Value);
            if (contactForm == null)
            {
                return HttpNotFound();
            }
            return View(contactForm);
        }

        // POST: ContactForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _contactFormServices.Delete(id);
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
