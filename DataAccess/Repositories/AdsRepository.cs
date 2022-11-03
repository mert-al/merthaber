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

        public void Create(Ad reklam)
        {
            try
            {

                reklam.CreatedDate = DateTime.Now;
                reklam.UpdatedDate = DateTime.Now;
                _db.Ads.Add(reklam);
                _db.SaveChanges();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void Update(Ad reklam)
        //{
        //    try
        //    {

        //        reklam.UpdatedDate = DateTime.Now;
        //        _db.Entry(reklam).State = EntityState.Modified;
        //        _db.SaveChanges();


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
