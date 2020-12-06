using System;
using System.Linq;
using System.Collections.Generic;
using CryptoCAD.Domain.Repositories;
using CryptoCAD.Domain.Entities.Methods;
using CryptoCAD.Data.Storage.Abstractions;

namespace CryptoCAD.Data.Repositories
{
    public class MethodsRepository : IMethodsRepository
    {
        private readonly IStorageContext Context;

        public MethodsRepository(IStorageContext context)
        {
            Context = context;
        }

        public void Add(Method entity)
        {
            Context.Methods.Add(entity);
        }

        public void AddRange(IList<Method> entities)
        {
            foreach (var entity in entities)
            {
                Context.Methods.Add(entity);
            }
        }

        public Method Get(Guid id)
        {
            return Context.Methods.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Method> GetAll()
        {
            return Context.Methods;
        }

        public void Remove(Method entity)
        {
            Context.Methods.Remove(entity);
        }

        public void Update(Method entity)
        {
            Remove(entity);
            Add(entity);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}