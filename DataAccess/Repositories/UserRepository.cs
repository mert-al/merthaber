using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        private NewsDBContext _db;

        public UserRepository(NewsDBContext context)
        {
            _db = context;
        }

        public User Login(User user)
        {
            return _db.Users.Where(model => model.EMail == user.EMail && !model.isDeleted).FirstOrDefault();
        }

        public User GetByEmail(String Email)
        {
            try
            {
                return _db.Users.Where(model => model.EMail == Email && model.isActive && !model.isDeleted).FirstOrDefault();
            }
            catch (Exception ex)
            {
                    throw ex;
            }
        }

        public List<User> Search(String query, bool? isActive = false)
        {
            try
            {
                return _db.Users.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive) && (model.Name + " " + model.Surname).Contains(query)).OrderByDescending(model => model.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetActiveByRole(int RoleId)
        {
            try
            {
                return _db.Roles.Where(model => !model.isDeleted && model.Id == RoleId).FirstOrDefault().Users.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public User Update(User user , int SelectedRole)
        {
            try
            {
                ClearRole(user.Id);
                _db.Roles.Find(SelectedRole).Users.Add(user);
                _db.Users.Find(user.Id).Role = _db.Roles.Find(SelectedRole);
                user.News = _db.Users.Find(user.Id).News;
                user.Videos = _db.Users.Find(user.Id).Videos;
                user.UpdatedDate = DateTime.Now;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return user;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Create(User user , int RoleId)
        {
            try
            {
                user.Role = _db.Roles.Find(RoleId);
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ClearRole(int UserId)
        {
            _db.Database.ExecuteSqlCommand("EXEC clearUserRole @userId = " + UserId);
        }

    }
}
