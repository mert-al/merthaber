using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Repositories
{
    public class VideoRepository : GenericRepository<Video>
    {
        NewsDBContext _db;
        public VideoRepository(NewsDBContext context)
        {
            _db = context;
        }

        public List<Video> Search(String query, bool? isActive = false)
        {
            try
            {
                return _db.Videos.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive) && model.Title.Contains(query)).OrderByDescending(model => model.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Create(Video video, int userId, List<String> SelectedCategories)
        {
            try
            {
                if (SelectedCategories.Count > 0)
                {
                    foreach (String id in SelectedCategories)
                    {
                        video = AddCategories(video, int.Parse(id));
                    }
                }
                video.User = _db.Users.Find(userId);
                video.CreatedDate = DateTime.Now;
                video.UpdatedDate = DateTime.Now;
                _db.Videos.Add(video);
                _db.SaveChanges();
                video.url += video.Id;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetVideoReklam(String videoUrl)
        {
            try
            {
                _db.Videos.Find(videoUrl).VideoTime.ToList();
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
                video.User = _db.Users.Find(video.User_Id);
                video.Categories.Clear();
                ClearCategories(video.Id);
                if (SelectedCategories != null)
                {
                    if (SelectedCategories.Count > 0)
                    {
                        foreach (String id in SelectedCategories)
                        {
                            video = AddCategories(video, int.Parse(id));
                        }
                    }
                }
                video.UpdatedDate = DateTime.Now;
                _db.Entry(video).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Review> GetReviewsByVideoId(int Id)
        {
            try
            {
                List<Review> reviews = _db.Videos.Find(Id).Reviews.ToList();
                return reviews;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void GetTimeVideoId(string videoUrl, string videoTime)
        {
            try
            {
                if (videoUrl != null)
                {
                    _db.Database.ExecuteSqlCommand("exec AddVideoTime @VideoTime =" + videoTime);
                }

           
                    
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void UpdateHit(String videoUrl)
        {
            try
            {
                _db.Videos.Where(model => model.url == videoUrl).FirstOrDefault().Hit++;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public Video AddCategories(Video video, int CategoryId)
        {
            _db.Categories.Find(CategoryId).Videos.Add(video);
            video.Categories.Add(_db.Categories.Find(CategoryId));
            return video;
        }

        public void ClearCategories(int videoId)
        {
            _db.Database.ExecuteSqlCommand("EXEC clearCategoriesFromVideo @VideoId = " + videoId);
        }
        public List<Video> GetVideoByCategories(List<int> categoriIds)
        {
            try
            {
                return _db.Videos.Where(model => !model.isDeleted && model.isActive && model.Categories.Select(category => category.Id).ToList().Any(item => categoriIds.Contains(item))).ToList().Take(5).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
