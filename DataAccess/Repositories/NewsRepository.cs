using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class NewsRepository : GenericRepository<News>
    {
        private NewsDBContext _db;
        public NewsRepository(NewsDBContext context) 
        {
            _db = context;
        }


        //public List<News> GetNews()
        //{
        //    try
        //    {
        //        List<News> news = _db.News.Where(model => !model.isDeleted).OrderByDescending(model => model.PublishDate).ToList();
        //        return news;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public List<News> GetHomeNewsWithoutTrendingNow()
        {
            try
            {
                return _db.News
                    .Where(model => model.isActive && !(model.TrendingNow == true && model.PublishDate < DateTime.UtcNow.AddHours(3) && model.PublishDate > DateTime.UtcNow.AddHours(-3)))
                    .OrderByDescending(model => model.PublishDate)
                    .ToList();
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
                return _db.News.Where(model => !model.isDeleted && model.isActive && model.url == url).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<News> GetByCategoryUrl(String url)
        {
            try
            {
                return _db.Categories.Where(model => !model.isDeleted && model.isActive && model.url == url).FirstOrDefault().News.ToList();
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
                return _db.Categories.Find(categoryId).News.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<News> GetByCategories(List<int> categoriIds)
        {
            try
            {
                return _db.News.Where(model => !model.isDeleted && model.isActive && model.Categories.Select(category => category.Id).ToList().Any(item => categoriIds.Contains(item))).ToList().Take(5).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Review> GetReviewsByNewsId(int? Id)
        {
            try
            {
                List<Review> reviews = _db.News.Find(Id).Reviews.ToList();
                return reviews;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<News> Search(String query, bool? isActive = false)
        {
            try
            {
                return _db.News.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive) && model.Title.Contains(query)).OrderByDescending(model => model.PublishDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Create(News news, int userId, List<String> SelectedCategories)
        {
            try
            {
                if (SelectedCategories.Count > 0)
                {
                    foreach (String id in SelectedCategories)
                    {
                        news = AddCategories(news, int.Parse(id));
                    }
                }
                news.User = _db.Users.Find(userId);
                news.CreatedDate = DateTime.Now;
                news.UpdatedDate = DateTime.Now;
                news.User_Id = userId;
                _db.News.Add(news);
                _db.SaveChanges();
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
                news.User = _db.Users.Find(news.User_Id);
                news.Categories.Clear();
                ClearCategories(news.Id);
                if (SelectedCategories != null)
                {
                    if (SelectedCategories.Count > 0)
                    {
                        foreach (String id in SelectedCategories)
                        {
                            news = AddCategories(news, int.Parse(id));
                        }
                    }
                }
                news.UpdatedDate = DateTime.Now;
                _db.Entry(news).State = EntityState.Modified;
                _db.SaveChanges();
                return news;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateHit(String newsUrl)
        {
            try
            {
                _db.News.Where(model => model.url == newsUrl).FirstOrDefault().Hit++;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public News AddCategories(News news, int CategoryId)
        {
            try
            {
                _db.Categories.Find(CategoryId).News.Add(news);
                news.Categories.Add(_db.Categories.Find(CategoryId));
                return news;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ClearCategories(int newsId)
        {
            try
            {
                _db.Database.ExecuteSqlCommand("EXEC clearCategoriesFromNews @NewsId = " + newsId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        // Search news 
        //if (!String.IsNullOrEmpty(searchQuery))
        //    {
        //        List<News> searchedNews = news.Where(model => model.Title.ToLower().Contains(searchQuery.ToLower())).ToList();

        //        return searchedNews;
        //    }
    }
}
