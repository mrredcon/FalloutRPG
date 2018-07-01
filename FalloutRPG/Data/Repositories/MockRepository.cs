using FalloutRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FalloutRPG.Data.Repositories
{
    public class MockRepository<T> : IRepository<T> where T : BaseModel, new()
    {
        private List<T> entities = new List<T>();

        public IQueryable<T> Query => entities.AsQueryable();

        public Task<IQueryable<T>> QueryAsync => throw new NotImplementedException();

        public void Add(T entity)
        {
            entities.Add(entity);
        }

        public Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            entities.Remove(entity);
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public List<T> FetchAll() => entities;

        public Task<List<T>> FetchAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Save(T entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}