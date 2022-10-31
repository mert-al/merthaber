using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using DataAccess.Repositories;
using HaberSitesiAdmin.Models;

namespace HaberSitesiAdmin.Services
{
    public class NewsServices
    {
        private UnitOfWork _unitOfWork;
        public NewsServices()
        {
           _unitOfWork = new UnitOfWork();
        }
        
        public List<News> GetAll()
        {
            return _unitOfWork.NewsRepository.GetAll().OrderByDescending(model => model.PublishDate).ToList();
        }

        public News Get(int? Id)
        {
            if (Id == null)
            {
                return null;
            }
            return _unitOfWork.NewsRepository.Get(Id.Value);
        }

        public PageDTO<News> GetPage(PageDTO<News> pageDTO)
        {
            try
            {
                var searchStatus = String.IsNullOrWhiteSpace(pageDTO.SearchQuery);
                var items = _unitOfWork.NewsRepository.GetAll().Where(model =>( searchStatus || model.Title.ToLower().Contains(pageDTO.SearchQuery.ToLower())));
                pageDTO.Pager = new Pager(items.Count(), pageDTO.Index, pageDTO.PageSize, 10);
                if(items.Count() != 0)
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

        public List<Review> GetReviewsByNewsId(int? Id)
        {
            if (Id == null)
            {
                return null;
            }
            return _unitOfWork.NewsRepository.GetReviewsByNewsId(Id.Value);
        }

        public List<SelectListItem> GetCategorySelectList()
        {
            List<Category> categories = _unitOfWork.CategoryRepository.GetAll(true).ToList();
            List<SelectListItem> categorySelectList = new List<SelectListItem>();
            foreach (Category item in categories)
            {
                categorySelectList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                });
            }
            return categorySelectList;
        }

        public List<SelectListItem> GetSelectListByNews(News news)
        {
            List<Category> categories = _unitOfWork.CategoryRepository.GetAll(true).ToList();
            List<SelectListItem> categorySelectList = new List<SelectListItem>();
            foreach (Category item in categories)
            {
                categorySelectList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = news.Categories.Where(x => x.Id == item.Id).Any()
                });
            }
            return categorySelectList;
        }

        public void Create(News news , List<String> SelectedCategories)
        {
            try
            {
                User user = (User)HttpContext.Current.Session["User"];

                if (user.Role.Id == 3)
                    news.isActive = false;
                _unitOfWork.NewsRepository.Create(news, user.Id, SelectedCategories);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public News Update(News news, List<String> SelectedCategories)
        {
            try
            {
                return _unitOfWork.NewsRepository.Update(news, SelectedCategories);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public String UpdateImage(String fileName , HttpPostedFileBase file)
        {
            string _path = Path.Combine(HttpContext.Current.Server.MapPath("~/Storage/News/Original/"), fileName+".png");
            string _url = Path.Combine("/Storage/News/Original", fileName +".png");
            file.SaveAs(_path);
            return _url;
        }

        public String CreateCroppedImage(String fileName,String base64,String folder)
        {
            try
            {
                if (!String.IsNullOrEmpty(base64))
                {
                    string fileType = base64.Split(',')[0].Split('/')[1].Split(';')[0];
                    byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
                    String Path = "~/Storage/News/" + folder + "/" + fileName + "." + fileType;
                    using (FileStream stream = new FileStream(HttpContext.Current.Server.MapPath(Path), FileMode.Create))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                    return "/Storage/News/" + folder + "/" + fileName + "."+ fileType;
                }
                throw new Exception();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String UpdateCroppedImage(String fileName,String base64 , String CurrentImg , String Path)
        {
            try
            {
                if (String.IsNullOrEmpty(base64))
                {
                    return CurrentImg;
                }
                else
                {
                    return CreateCroppedImage(fileName, base64,Path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            _unitOfWork.NewsRepository.Delete(id);
        }

    }
}