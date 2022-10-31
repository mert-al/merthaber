using DataAccess;
using DataAccess.Repositories;
using HaberSitesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSitesi.Services
{
    public class AlbumServices
    {
        UnitOfWork _unitOfWork;
        public AlbumServices()
        {
            _unitOfWork = new UnitOfWork();
        }
        public List<Category> GetCategoryMenu()
        {
            try
            {
                return _unitOfWork.CategoryRepository.GetAll(true).Take(5).OrderBy(model => model.Status).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public HomePageDto<Album> GetdtoHomePage()
        {
            try
            {
                var now = DateTime.UtcNow.AddHours(3);
                HomePageDto<Album> homePageData = new HomePageDto<Album>();
                IQueryable<Album> albums = _unitOfWork.AlbumRepository.GetAll(true).Where(model =>model.PublishDate <= now && model.Categories.Count > 0).OrderByDescending(model => model.Hit);
                homePageData.TopHit = albums.Take(6).ToList();
                homePageData.Items = albums.Skip(6).OrderByDescending(model => model.PublishDate).ToList();
                //homePageData.TrendingNow = GetTrendingNow(albums);
                //homePageData.Videos = GetVideos();

                return homePageData;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DetailsDto<Album> GetAlbumsDetails(String categoryUrl, String albumUrl)
        
        {
            try
            {
                List<int> categoryIds = new List<int>();
                DetailsDto<Album> dtoAlbumDetails = new DetailsDto<Album>();

                var category = _unitOfWork.CategoryRepository.GetAlbumsByUrl(categoryUrl);
                if (category == null)
                {
                    return dtoAlbumDetails;
                }
                var albums = category.Where(model => model.PublishDate <= DateTime.UtcNow.AddHours(3) && model.url == albumUrl).FirstOrDefault();
                if (albums == null)
                {
                    return dtoAlbumDetails;
                }
                dtoAlbumDetails.Item = albums;
                foreach (Category item in dtoAlbumDetails.Item.Categories)
                {
                    categoryIds.Add(item.Id);
                }
                
                dtoAlbumDetails.Sidebar = _unitOfWork.AlbumRepository.GetAlbumByCategories(categoryIds);

                if (dtoAlbumDetails.Item != null)
                {
                    _unitOfWork.AlbumRepository.UpdateHit(albumUrl);
                }
                dtoAlbumDetails.Item.AlbumIMGs = dtoAlbumDetails.Item.AlbumIMGs.Where(model => !model.isDeleted).ToList();
                return dtoAlbumDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}