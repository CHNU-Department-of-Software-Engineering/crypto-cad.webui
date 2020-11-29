using System;
using System.Linq;
using System.Collections.Generic;

namespace CryptoCAD.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(Guid id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void AddRange(IList<T> entities);
        void Remove(T entity);
        void Update(T entity);
        void SaveChanges();
    }
}