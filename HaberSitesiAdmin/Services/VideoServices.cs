using DataAccess;
using DataAccess.Repositories;
using HaberSitesiAdmin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesiAdmin.Services
{
    public class VideoServices
    {
        UnitOfWork _unitOfWork;
        public VideoServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public List<Video> GetAll()
        {
            try
            {
                return _unitOfWork.VideoRepository.GetAll().OrderBy(model => model.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Video Get(int id)
        {
            try
            {
                return _unitOfWork.VideoRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PageDTO<Video> GetPage(PageDTO<Video> pageDTO)
        {
            try
            {
                var searchStatus = String.IsNullOrWhiteSpace(pageDTO.SearchQuery);
                var items = _unitOfWork.VideoRepository.GetAll().Where(model => (searchStatus || model.Title.Contains(pageDTO.SearchQuery)));
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

        public List<Review> GetReviewsByVideoId(int? Id)
        {
            if (Id == null)
            {
                return null;
            }
            return _unitOfWork.VideoRepository.GetReviewsByVideoId(Id.Value);
        }
        public void GetTimeVideoUrl(string videoUrl,string videoTime)
        {
            try
            {
                _unitOfWork.VideoRepository.GetTimeVideoId(videoUrl, videoTime);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
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

        public List<SelectListItem> GetSelectListByVideo(Video video)
        {
            List<Category> categories = _unitOfWork.CategoryRepository.GetAll(true).ToList();
            List<SelectListItem> categorySelectList = new List<SelectListItem>();
            foreach (Category item in categories)
            {
                categorySelectList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = video.Categories.Where(x => x.Id == item.Id).Any()
                });
            }
            return categorySelectList;
        }


        public void Create(Video video, List<String> SelectedCategories)
        {
            try
            {
                User user = (User)HttpContext.Current.Session["User"];

                if (user.Role.Id == 3)
                    video.isActive = false;

                _unitOfWork.VideoRepository.Create(video, user.Id, SelectedCategories);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(Video video, List<String> SelectedCategories)
        {
            try
            {
                _unitOfWork.VideoRepository.Update(video, SelectedCategories);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _unitOfWork.VideoRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public String SaveVideo(String fileName, HttpPostedFileBase file)
        {
            string _path = Path.Combine(HttpContext.Current.Server.MapPath("~/Storage/Video/Videos"),file.FileName);
            string _url = Path.Combine("/Storage/Video/Videos", file.FileName);
            file.SaveAs(_path);
            return _url;
        }

        public void DeleteVideo(String filePath)
        {
            try {

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (Exception ex) {   
            }
        }

        public String UpdateImage(String fileName, HttpPostedFileBase file)
        {
            string _path = Path.Combine(HttpContext.Current.Server.MapPath("~/Storage/Video/Original/"), fileName +".png" );
            string _url = Path.Combine("/Storage/Video/Original", fileName + ".png");
            file.SaveAs(_path);
            return _url;
        }

        public String CreateCroppedImage(String fileName, String base64, String folder)
        {
            try
            {
                if (!String.IsNullOrEmpty(base64))
                {
                    string fileType = base64.Split(',')[0].Split('/')[1].Split(';')[0];
                    byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
                    String Path = "~/Storage/Video/" + folder + "/" + fileName + "." + fileType;
                    using (FileStream stream = new FileStream(HttpContext.Current.Server.MapPath(Path), FileMode.Create))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                    return "/Storage/Video/" + folder + "/" + fileName + "." + fileType;
                }
                throw new Exception();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String UpdateCroppedImage(String fileName, String base64, String CurrentImg, String Path)
        {
            try
            {
                if (String.IsNullOrEmpty(base64))
                {
                    return CurrentImg;
                }
                else
                {
                    return CreateCroppedImage(fileName, base64, Path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}