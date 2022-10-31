using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        NewsDBContext _db;
        public RoleRepository(NewsDBContext context)
        {
            _db = context;
        }


        public List<User> GetUsers(int id)
        {
            try
            {
                return _db.Roles.Find(id).Users.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Role> Search(string query, bool? isActive = false)
        {
            try
            {
                return _db.Roles.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive) && model.Name.Contains(query)).OrderByDescending(model => model.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
