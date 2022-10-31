using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        NewsDBContext _db;
        public CategoryRepository(NewsDBContext context)
        {
            _db = context;
        }
        public List<News> GetNewsByUrl(string url, bool? isActive = false)
        {
            try
            {
                var now = DateTime.UtcNow.AddHours(3);
                return _db.Categories.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive) && model.url == url).OrderByDescending(model => model.CreatedDate).FirstOrDefault().News.Where(model => model.PublishDate <= now && !model.isDeleted && model.isActive).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public string GetNameByUrl(string url)
        {
            try
            {
                var categoryName = _db.Categories.Where(model => !model.isDeleted && model.isActive && model.url == url).FirstOrDefault();
                return categoryName == null ? null : categoryName.Name;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Album> GetAlbumsByUrl(string url, bool? isActive = false)
        {
            try
            {
                var now = DateTime.UtcNow.AddHours(3);
                return _db.Categories.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive) && model.url == url).OrderByDescending(model => model.CreatedDate).FirstOrDefault().Albums.Where(model => model.PublishDate <= now && !model.isDeleted && model.isActive).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<Video> GetVideoByUrl(string url, bool? isActive = false)
        {
            try
            {
                var now = DateTime.UtcNow.AddHours(3);
                return _db.Categories.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive) && model.url == url).OrderByDescending(model => model.CreatedDate).FirstOrDefault().Videos.Where(model => model.PublishDate <= now && !model.isDeleted && model.isActive).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }



        public List<Category> Search(string query,bool? isActive = false)
        {
            try
            {
                return _db.Categories.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive) && model.Name.Contains(query)).OrderByDescending(model => model.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
