using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AlbumRepository : GenericRepository<Album>
    {
        NewsDBContext _db;
        public AlbumRepository(NewsDBContext context)
        {
            _db = context;
        }
        public void Create(Album album, int userId, List<String> SelectedCategories , List<AlbumIMG> albumIMGs)
        {
            try
            {
                if (SelectedCategories.Count > 0)
                {
                    foreach (String id in SelectedCategories)
                    {
                        album = AddCategories(album, int.Parse(id));
                    }
                }
                album.User = _db.Users.Find(userId);
                album.CreatedDate = DateTime.Now;
                album.UpdatedDate = DateTime.Now;
                if (album.AlbumIMGs != null)
                {
                    if (album.AlbumIMGs.Count() > 0)
                    {
                        foreach (AlbumIMG item in album.AlbumIMGs)
                        {
                            item.CreatedDate =  DateTime.Now;
                            item.UpdatedDate =  DateTime.Now;
                        }
                    }
                }
                album.UserId = userId;
                _db.Albums.Add(album);
                _db.SaveChanges();
                album.url += album.Id;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Album AddCategories(Album album, int CategoryId)
        {
            try
            {
                _db.Categories.Find(CategoryId).Albums.Add(album);
                album.Categories.Add(_db.Categories.Find(CategoryId));
                return album;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Review> GetReviewsByAlbumId(int Id)
        {
            try
            {
                List<Review> reviews = _db.Albums.Find(Id).Reviews.ToList();
                return reviews;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AlbumIMG AddIMG(AlbumIMG albumIMG)
        {
            try
            {
                _db.AlbumIMGs.Add(albumIMG);
                return albumIMG;

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
                album.User = _db.Users.Find(album.UserId);
                album.Categories.Clear();
                ClearCategories(album.Id);
                ClearImages(album.Id);
                _db.SaveChanges();
                if (SelectedCategories != null)
                {
                    if (SelectedCategories.Count > 0)
                    {
                        foreach (String id in SelectedCategories)
                        {
                            album = AddCategories(album, int.Parse(id));
                        }
                    }
                }
                album.UpdatedDate = DateTime.Now;
                album.url += album.Id;
                _db.Entry(album).State = EntityState.Modified;
                _db.SaveChanges();
                return album;
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
                _db.Albums.Where(model => model.url == newsUrl).FirstOrDefault().Hit++;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ClearCategories(int albumId)
        {
            try
            {
                _db.Database.ExecuteSqlCommand("EXEC clearCategoriesFromAlbum @AlbumId = " + albumId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void ClearImages(int albumId)
        {
            try
            {
                _db.Database.ExecuteSqlCommand("EXEC clearImagesFromAlbum @AlbumId = " + albumId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Album> GetAlbumByCategories(List<int> categoriIds)
        {
            try
            {
                return _db.Albums.Where(model => !model.isDeleted && model.isActive && model.Categories.Select(category => category.Id).ToList().Any(item => categoriIds.Contains(item))).ToList().Take(5).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
