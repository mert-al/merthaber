
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Repositories;
using DataAccess;
using HaberSitesi.Models;

namespace HaberSitesi.Services
{
    public class NewsServices
    {
        UnitOfWork _unitOfWork;
        public NewsServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<News> GetByPublishDate()
        {
            try
            {
                return _unitOfWork.NewsRepository.GetAll().OrderByDescending(model => model.PublishDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<News> GetByCategory(int categoryId)
        {
            try
            {
                return _unitOfWork.NewsRepository.GetByCategory(categoryId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public News Get(int id)
        {
            try
            {
                return _unitOfWork.NewsRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public News GetInfinite(string url, string[] news)
        {
            try
            {
                return _unitOfWork.NewsRepository.GetByUrl(url).Categories.Select(category => category.News).ToList()[0].ToList().Where(model => !news.Contains(model.url)).OrderByDescending(model => model.PublishDate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public News GetByUrl(String url)
        {
            try
            {
                return _unitOfWork.NewsRepository.GetByUrl(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<News> GetAllWithoutTrendingNow()
        {
            try
            {
                return _unitOfWork.NewsRepository.GetAll().ToList()
                    .Where(model => (model.PublishDate <= DateTime.UtcNow.AddHours(3)) && model.isActive && !(model.TrendingNow == true && model.PublishDate < DateTime.UtcNow.AddHours(3) && model.PublishDate > DateTime.UtcNow.AddHours(-3))).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<News> GetTopHitWithoutTrendingNow(List<News> news)
        {
            try
            {
                return news
                    .Where(model => model.isActive && model.PublishDate <= DateTime.Now && !(model.TrendingNow == true && model.PublishDate < DateTime.UtcNow.AddHours(3) && model.PublishDate > DateTime.UtcNow
                    .AddHours(-3))).OrderByDescending(model => model.Hit)
                    .OrderByDescending(model => model.Hit)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Video> GetVideos()
        {
            try
            {
                return _unitOfWork.VideoRepository.GetAll(true).Where(model => model.isActive).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<News> GetTrendingNow()
        {
            try
            {
                return _unitOfWork.NewsRepository.GetAll(true).ToList()
                    .Where(model => (model.PublishDate <= DateTime.UtcNow.AddHours(3)) && model.isActive && (model.TrendingNow == true && model.PublishDate < DateTime.UtcNow.AddHours(3) && model.PublishDate > DateTime.UtcNow.AddHours(-3)))
                    .OrderByDescending(model => model.PublishDate)
                    .ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IndexPageDto GetdtoHomePage()
        {
            try
            {
                var now = DateTime.UtcNow.AddHours(3);
                //{ model.Title, model.Description,model.Img,model.PublishDate,model.Hit,model.url,model.MainSliderIMG,model.SidebarIMG,model.SliderBottomIMG,model.DetailsIMG,model.OtherIMG,model.BestWeeklyIMG,model.BestWeeklySmIMG,model.Categories}
                IndexPageDto homePageData = new IndexPageDto();
                //List<News> news = GetAllWithoutTrendingNow().ToList();
                List<NewsAlbumMix> Items = GetAllWithoutTrendingNow().Where(model => model.Categories.Count > 0).OrderByDescending(model=>model.PublishDate).Select(model => new NewsAlbumMix()
                {
                    Title = model.Title,
                    Description = model.Description,
                    url = model.url,
                    path = "",
                    PublishDate = model.PublishDate,
                    Hit = model.Hit,
                    Categories = model.Categories,
                    Img = model.Img,
                    BestWeeklyIMG = model.BestWeeklyIMG,
                    BestWeeklySmIMG = model.BestWeeklySmIMG,
                    MainSliderIMG = model.MainSliderIMG,
                    SliderBottomIMG = model.SliderBottomIMG,
                    DetailsIMG = model.DetailsIMG,
                    SidebarIMG = model.SidebarIMG,
                    OtherIMG = model.OtherIMG
                }).ToList();
                Items.AddRange(_unitOfWork.AlbumRepository.GetAll(true).Where(model => model.Categories.Count > 0 && model.PublishDate <= now).Select(model => new NewsAlbumMix() 
                {
                    Title = model.Title,
                    Description = model.Description,
                    url = model.url,
                    path = "galeri/",
                    PublishDate = model.PublishDate,
                    Hit = model.Hit,
                    Categories = model.Categories,
                    Img = model.Img,
                    BestWeeklyIMG = model.BestWeeklyIMG,
                    BestWeeklySmIMG = model.BestWeeklySmIMG,
                    MainSliderIMG = model.MainSliderIMG,
                    SliderBottomIMG = model.SliderBottomIMG,
                    DetailsIMG = model.DetailsIMG,
                    SidebarIMG = model.SidebarIMG,
                    OtherIMG = model.OtherIMG
                }).ToList().Where(model => (model.PublishDate <= DateTime.UtcNow.AddHours(3))));
                List<Video> VideoItems = new List<Video>();
                VideoItems = _unitOfWork.VideoRepository.GetAll(true).Where(model => model.Categories.Count > 0).OrderByDescending(model => model.PublishDate).ToList();
                homePageData.Videos = VideoItems.Take(6).ToList();
                Items.AddRange(VideoItems.Skip(6).Select(model => new NewsAlbumMix()
                {
                    Title = model.Title,
                    Description = model.Description,
                    url = model.url,
                    path = "video/",
                    PublishDate = model.PublishDate,
                    Hit = model.Hit,
                    Categories = model.Categories,
                    Img = model.Img,
                    BestWeeklyIMG = model.BestWeeklyIMG,
                    BestWeeklySmIMG = model.BestWeeklySmIMG,
                    MainSliderIMG = model.MainSliderIMG,
                    SliderBottomIMG = model.SliderBottomIMG,
                    DetailsIMG = model.DetailsIMG,
                    SidebarIMG = model.SidebarIMG,
                    OtherIMG = model.OtherIMG
                }).ToList().Where(model => (model.PublishDate <= DateTime.UtcNow.AddHours(3))));
                Items = Items.OrderByDescending(model => model.Hit).ToList();
                homePageData.TopHit = Items.GetRange(0, 6);
                homePageData.Items = Items.Skip(6).OrderByDescending(model => model.PublishDate).ToList();
                homePageData.TrendingNow = GetTrendingNow();                
                return homePageData;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DetailsDto<News> GetNewsDetails(String categoryUrl, String newsUrl)
        {
            try
            {
                List<int> categoryIds = new List<int>();
                DetailsDto<News> dtoNewsDetails = new DetailsDto<News>();

                dtoNewsDetails.TrendingNow = GetTrendingNow();
                var category = _unitOfWork.CategoryRepository.GetNewsByUrl(categoryUrl);
                if(category == null)
                {
                    return dtoNewsDetails;
                }
                var news = category.Where(model => model.PublishDate <= DateTime.UtcNow.AddHours(3) && model.url == newsUrl).FirstOrDefault();
                if (news == null)
                {
                    return dtoNewsDetails;
                }
                dtoNewsDetails.Item = news;
                foreach (Category item in dtoNewsDetails.Item.Categories)
                {
                    categoryIds.Add(item.Id);
                }

                dtoNewsDetails.Sidebar = _unitOfWork.NewsRepository.GetByCategories(categoryIds).Where(model=>model.Id  !=dtoNewsDetails.Item.Id).ToList();

                if(dtoNewsDetails.Item != null)
                {
                    _unitOfWork.NewsRepository.UpdateHit(newsUrl);
                }
                return dtoNewsDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<News> SearchNews(String query)
        {
            try
            {
                return _unitOfWork.NewsRepository.Search(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}