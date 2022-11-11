using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
   public class AdsRepository : GenericRepository<Ad>
    {
        NewsDBContext _db;
        public AdsRepository(NewsDBContext context)
        {
            _db = context;
        }

        public void Create(Ad reklam ,int userId)
        {
            try
            {
                reklam.User = _db.Users.Find(userId);
                reklam.CreatedDate = DateTime.Now;
                reklam.UpdatedDate = DateTime.Now;
                reklam.User_ID = userId;
                _db.Ads.Add(reklam);
                _db.SaveChanges();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateHit(Ad reklam)
        {
            try
            {
                _db.Ads.Where(model => model.Preroll == reklam.Preroll ).FirstOrDefault().PrerollHit++;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update(Ad reklam, int userId)
        {
            try
            {
                reklam.User = _db.Users.Find(userId);
                reklam.UpdatedDate = DateTime.Now;
                reklam.User_ID = userId;
                _db.Entry(reklam).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void UpdateHit(Ad reklam)
        //{
        //    try
        //    {
        //        _db.Ads.Where(model => model.PrerolTitle == reklam.T).FirstOrDefault().Hit++;
        //        _db.SaveChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



    }
}
