using FalloutRPG.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FalloutRPG.Data.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseModel, new()
    {
        private readonly BotContext _context;

        public EfRepository(BotContext context)
        {
            _context = context;
        }

        public IQueryable<T> Query => _context.Set<T>().AsQueryable();

        public void Add(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public List<T> FetchAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<List<T>> FetchAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public void Save(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        public async Task SaveAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
