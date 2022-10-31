using DataAccess;
using DataAccess.Repositories;
using HaberSitesiAdmin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace HaberSitesiAdmin.Services
{
    public class AlbumServices
    {
        private UnitOfWork _unitOfWork;

        public AlbumServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<Album> GetAll()
        {
            try
            {
                return _unitOfWork.AlbumRepository.GetAll().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Album Get(int id)
        {
            try
            {
                return _unitOfWork.AlbumRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PageDTO<Album> GetPage(PageDTO<Album> pageDTO)
        {
            try
            {
                var searchStatus = String.IsNullOrWhiteSpace(pageDTO.SearchQuery);
                var items = _unitOfWork.AlbumRepository.GetAll().Where(model => (searchStatus || model.Title.ToLower().Contains(pageDTO.SearchQuery.ToLower())));
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

        internal List<Review> GetReviewsByAlbumId(int? Id)
        {
            if (Id == null)
            {
                return null;
            }
            return _unitOfWork.AlbumRepository.GetReviewsByAlbumId(Id.Value);
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

        public List<SelectListItem> GetSelectListByAlbum(Album album)
        {
            List<Category> categories = _unitOfWork.CategoryRepository.GetAll(true).ToList();
            List<SelectListItem> categorySelectList = new List<SelectListItem>();
            foreach (Category item in categories)
            {
                categorySelectList.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = album.Categories.Where(x => x.Id == item.Id).Any()
                });
            }
            return categorySelectList;
        }

        public void Create(Album album, List<String> SelectedCategories)
        {
            try
            {
                User user = (User)HttpContext.Current.Session["User"];

                if (user.Role.Id == 3)
                    album.isActive = false;
                _unitOfWork.AlbumRepository.Create(album,user.Id, SelectedCategories,album.AlbumIMGs.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String UpdateImage(String fileName, HttpPostedFileBase file)
        {
            string _path = Path.Combine(HttpContext.Current.Server.MapPath("~/Storage/Albums/Original/"), fileName + ".png");
            string _url = Path.Combine("/Storage/Albums/Original", fileName + ".png");
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
                    String Path = "~/Storage/Albums/" + folder + "/" + fileName + "." + fileType;
                    using (FileStream stream = new FileStream(HttpContext.Current.Server.MapPath(Path), FileMode.Create))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                    return "/Storage/Albums/" + folder + "/" + fileName + "." + fileType;
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

        public Album Update(Album album, List<String> SelectedCategories)
        {
            try
            {
                return _unitOfWork.AlbumRepository.Update(album, SelectedCategories);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void Delete(int id)
        {
            _unitOfWork.AlbumRepository.Delete(id);
        }

        public void UpdateAlbumImages(List<UpdatedImages> updatedImages)
        {
            foreach(UpdatedImages item in updatedImages)
            {
                if(item != null)
                {
                    if(item.File != null)
                    {
                        var fileName = _unitOfWork.AlbumIMGRepository.Get(item.AlbumImgId).IMG;
                        string _path = Path.Combine(HttpContext.Current.Server.MapPath("~/Storage/Albums/Original/"), fileName);
                        //string _url = Path.Combine("/Storage/Albums/Original", fileName);
                        item.File.SaveAs(HttpContext.Current.Server.MapPath(fileName));
                    }
                }
            }
        }

        private String CreateAlbumImages(UpdatedImages updatedImages)
        {
            try
            {
                var fileName = _unitOfWork.AlbumIMGRepository.Get(updatedImages.AlbumImgId).IMG;
                string _path = Path.Combine(HttpContext.Current.Server.MapPath("~/Storage/Albums/Original/"), fileName);
                string _url = Path.Combine("/Storage/Albums/Original",fileName);
                updatedImages.File.SaveAs(_path);
                return _url;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}