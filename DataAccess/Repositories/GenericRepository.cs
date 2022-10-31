using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Models
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private NewsDBContext _db;
        private DbSet<T> table = null;

        public GenericRepository(NewsDBContext _db)
        {
            this._db = _db;
            table = _db.Set<T>();
        }

        public GenericRepository()
        {
            this._db = new NewsDBContext();
            table = _db.Set<T>();
        }

        public IQueryable<T> GetAll(bool? isActive = false)
        {
            return table.Where(model => !model.isDeleted && (model.isActive == isActive || model.isActive));
        }

        public T Get(long id, bool? isActive = false)
        {
            return table.Where(model => !model.isDeleted && (model.isActive || model.isActive == isActive) && model.Id == id).FirstOrDefault();
        }

        public void Insert(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow.AddHours(3);
            entity.UpdatedDate = DateTime.UtcNow.AddHours(3);
            table.Add(entity);
            _db.SaveChanges();
        }

        public void Update(T entity)
        {
            entity.UpdatedDate = DateTime.UtcNow.AddHours(3);
            table.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            T existing = table.Find(id);
            existing.isDeleted = true;
            existing.UpdatedDate = DateTime.UtcNow.AddHours(3);
            _db.SaveChanges();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
