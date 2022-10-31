using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ReviewRepository : GenericRepository<Review>
    {
        NewsDBContext _db;
        public ReviewRepository(NewsDBContext context)
        {
            _db = context;
        }

        public List<Review> Search(string query, bool? isActive = false)
        {
            try
            {
                return _db.Reviews.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive) && model.Title.Contains(query)).OrderByDescending(model => model.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
