using DataAccess;
using DataAccess.Repositories;
using HaberSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesi.Services
{
    public class CategoryServices
    {
        UnitOfWork _unitOfWork;
        public CategoryServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public String GetNameByUrl(string url)
        {
            try
            {
                
                return _unitOfWork.CategoryRepository.GetNameByUrl(url);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
        public List<Category> GetCategoryMenu()
        {
            try
            {
                return _unitOfWork.CategoryRepository.GetAll(true).OrderBy(model => model.Status).Take(4).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Album> GetAlbumsByUrl(String url)
        {
            try
            {
                return _unitOfWork.CategoryRepository.GetAlbumsByUrl(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<News> GetNewsByUrl(String url)
        {
            try
            {
                return _unitOfWork.CategoryRepository.GetNewsByUrl(url).OrderByDescending(model=>model.PublishDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Video> GetVideosByUrl(String url)
        {
            try
            {
                return _unitOfWork.CategoryRepository.GetVideoByUrl(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}