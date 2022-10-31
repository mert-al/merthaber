using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool? isActive = false);
        T Get(long id , bool? isActive = false);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
