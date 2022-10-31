using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using HaberSitesiAdmin.Attributes;
using HaberSitesiAdmin.Models;
using HaberSitesiAdmin.Services;

namespace HaberSitesiAdmin.Controllers
{
    [HandleError]
    public class UsersController : Controller
    {
        private UserServices _userServices;
        public UsersController()
        {
            _userServices = new UserServices();
        }


        // GET: Users
        [RoleAuth(Roles = "Yönetici")]
        public ActionResult Index(PageDTO<User> pageDTO)
        {
            pageDTO.Index = pageDTO.Index == 0 ? 1 : pageDTO.Index;
            pageDTO.PageSize = pageDTO.PageSize == 0 ? 10 : pageDTO.PageSize;
            return View(_userServices.GetPage(pageDTO));
        }

        // GET: Users/Details/5
        [RoleAuth()]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _userServices.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        [RoleAuth(Roles = "Yönetici")]
        public ActionResult Create()
        {
            ViewBag.Roles = _userServices.GetRolesSelectList();
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuth(Roles = "Yönetici")]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Img,EMail,Password,Birthday,isActive,isDeleted,CreatedDate,UpdatedDate")] User user, HttpPostedFileBase file, String SelectedRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ViewBag.Roles = _userServices.GetRolesSelectList();
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            user.Img = _userServices.UpdateUserImage(file);
                        }
                    }

                    if (SelectedRole != null)
                    {
                        _userServices.CreateUser(user, int.Parse(SelectedRole));
                    }
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return View(user);
        }

        // GET: Users/Edit/5
        [RoleAuth()]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _userServices.Get(id.Value);
            ViewBag.Roles = _userServices.GetRolesSelectListByUser(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Img,EMail,Password,Birthday,isActive,isDeleted,CreatedDate,UpdatedDate")] User user, HttpPostedFileBase file, String SelectedRole, String newPassword)
        {
            if (ModelState.IsValid)
            {
                user.Role_Id = int.Parse(SelectedRole);
                ViewBag.Roles = _userServices.GetRolesSelectListByUser(user);
                try
                {
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            user.Img = _userServices.UpdateUserImage(file);
                        }
                    }
                    _userServices.Update(user, int.Parse(SelectedRole), newPassword);
                }
                catch
                {

                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [RoleAuth(Roles = "Yönetici")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _userServices.Get(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RoleAuth(Roles = "Yönetici")]
        public ActionResult DeleteConfirmed(int id)
        {
            _userServices.DeleteUser(id);
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
