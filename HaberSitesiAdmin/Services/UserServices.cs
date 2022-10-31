using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using HaberSitesiAdmin.Models;
using DataAccess;
using DataAccess.Repositories;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace HaberSitesiAdmin.Services
{
    public class UserServices
    {
        private UnitOfWork _unitOfWork;
        public UserServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public LoginResult Login(User user)
        {
            LoginResult result = new LoginResult();
            result.Status = false;
            User currentUser = _unitOfWork.UserRepository.Login(user);
            if (currentUser == null)
            {
                result.Message = "Kullanici Bulunamadi";
            }
            else if (!currentUser.isActive)
            {
                result.Message = "Kullanici aktif degil";
            }
            else
            {
                if (MD5Hash(user.Password) == currentUser.Password)
                {
                    try
                    {
                        FormsAuthentication.SetAuthCookie(user.EMail, false);
                        HttpContext.Current.Session.Add("User", currentUser);
                        result.Message = "Basarili";
                        result.Status = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                else
                {
                    result.Message = "Sifre Hatali";
                }
            }
            return result;
        }

        public List<SelectListItem> GetRolesSelectList()
        {
            List<SelectListItem> roleList = new List<SelectListItem>();
            List<Role> roles = _unitOfWork.RoleRepository.GetAll(true).ToList();

            foreach (Role item in roles)
            {
                roleList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            return roleList;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Remove("User");
        }

        public void SessionFiller()
        {
            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName]; //Get the cookie by it's name
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
                string UserName = ticket.Name; //You have the UserName!
                User user = _unitOfWork.UserRepository.GetByEmail(UserName);
                if (user == null)
                {
                    FormsAuthentication.SignOut();
                }
                HttpContext.Current.Session.Add("User", user);
            }
        }
        public List<User> GetAll()
        {
            try
            {
                return _unitOfWork.UserRepository.GetAll().ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public User Get(int id)
        {
            try
            {
                return _unitOfWork.UserRepository.Get(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SelectListItem> GetRolesSelectListByUser(User user)
        {
            List<SelectListItem> roleList = new List<SelectListItem>();
            List<Role> roles = _unitOfWork.RoleRepository.GetAll(true).ToList();

            foreach (Role item in roles)
            {
                roleList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = user.Role_Id == item.Id
                });
            }
            return roleList;
        }


        public PageDTO<User> GetPage(PageDTO<User> pageDTO)
        {
            try
            {
                var searchStatus = String.IsNullOrWhiteSpace(pageDTO.SearchQuery);
                var items = _unitOfWork.UserRepository.GetAll().Where(model => (searchStatus || (model.Name + " " + model.Surname).Contains(pageDTO.SearchQuery)));
                pageDTO.Pager = new Pager(items.Count(), pageDTO.Index, pageDTO.PageSize, 10);
                if (items.Count() != 0)
                {
                    pageDTO.Items = items.OrderByDescending(model => model.CreatedDate).Skip(pageDTO.Pager.StartIndex).Take(pageDTO.PageSize).ToList();
                }
                return pageDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetActive(int id)
        {
            try
            {
                return _unitOfWork.UserRepository.Get(id, true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Update(User user, int selectedRole, String newPassword)
        {
            if (!String.IsNullOrEmpty(newPassword))
            {
                user.Password = MD5Hash(newPassword);
            }
            if (selectedRole != null)
            {
                user = _unitOfWork.UserRepository.Update(user, selectedRole);
                User activeUser = (User)HttpContext.Current.Session["User"];
                if (activeUser.Id == user.Id)
                {
                    HttpContext.Current.Session.Add("User", user);
                }
            }

        }

        public String UpdateUserImage(HttpPostedFileBase file)
        {
            string _FileName = Path.GetFileName(file.FileName);
            string _path = Path.Combine(HttpContext.Current.Server.MapPath("~/Storage/Profile"), _FileName);
            string _url = Path.Combine("/Storage/Profile", _FileName);
            file.SaveAs(_path);
            return _url;
        }

        public void CreateUser(User user, int roleId)
        {
            try
            {
                user.Password = MD5Hash(user.Password);
                user.CreatedDate = DateTime.Now;
                user.UpdatedDate = DateTime.Now;
                _unitOfWork.UserRepository.Create(user, roleId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteUser(int userId)
        {
            _unitOfWork.UserRepository.Delete(userId);
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }



    }
}
