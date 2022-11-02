using DataAccess;
using DataAccess.Repositories;
using HaberSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesi.Services
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
                return _unitOfWork.VideoRepository.GetAll().OrderByDescending(model => model.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetReklam(String videoUrl)
        {
            try
            {
                _unitOfWork.VideoRepository.GetVideoReklam(videoUrl);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public HomePageDto<Video> GetdtoHomePage()
        {
            try
            {
                var now = DateTime.UtcNow.AddHours(3);
                HomePageDto<Video> homePageData = new HomePageDto<Video>();
                IQueryable<Video> videos = _unitOfWork.VideoRepository.GetAll(true).Where(model => model.PublishDate <= now && model.Categories.Count > 0).OrderByDescending(model => model.Hit);
                homePageData.TopHit = videos.Take(6).ToList();
                homePageData.Items = videos.Skip(6).OrderByDescending(model => model.PublishDate).ToList();
                //homePageData.TrendingNow = GetTrendingNow(albums);
                //homePageData.Videos = GetVideos();

                return homePageData;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DetailsDto<Video> GetVideosDetails(String categoryUrl, String videoUrl)
        {
            try
            {
                List<int> categoryIds = new List<int>();
                DetailsDto<Video> dtoVideoDetails = new DetailsDto<Video>();

                var category = _unitOfWork.CategoryRepository.GetVideoByUrl(categoryUrl);
                if (category == null)
                {
                    return dtoVideoDetails;
                }
                var videos = category.Where(model => model.PublishDate <= DateTime.UtcNow.AddHours(3) && model.url == videoUrl).FirstOrDefault();
                if (videos == null)
                {
                    return dtoVideoDetails;
                }
                dtoVideoDetails.Item = videos;
                foreach (Category item in dtoVideoDetails.Item.Categories)
                {
                    categoryIds.Add(item.Id);
                }
                dtoVideoDetails.Sidebar = _unitOfWork.VideoRepository.GetVideoByCategories(categoryIds).Where(model=>model.Id != dtoVideoDetails.Item.Id).ToList();

               if (dtoVideoDetails.Item != null)
               {
                   _unitOfWork.VideoRepository.UpdateHit(videoUrl);
               }
                return dtoVideoDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}