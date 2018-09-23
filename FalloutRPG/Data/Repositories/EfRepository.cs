using FalloutRPG.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FalloutRPG.Data.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseModel
    {
        private readonly RpgContext _context;

        public EfRepository(RpgContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets an IQueryable object from the database.
        /// of set T
        /// </summary>
        public IQueryable<T> Query => _context.Set<T>().AsQueryable();

        /// <summary>
        /// Adds an entity of type T to the database.
        /// </summary>
        public void Add(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Adds an entity of type T to the database 
        /// asynchronously.
        /// </summary>
        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an entity of type T from the database.
        /// </summary>
        public void Delete(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes an entity of type T from the database
        /// asynchronously.
        /// </summary>
        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Fetch all the records of type T from the database.
        /// </summary>
        /// <remarks>
        /// Could be costly with a large amount of records.
        /// </remarks>
        public List<T> FetchAll()
        {
            return _context.Set<T>().ToList();
        }

        /// <summary>
        /// Fetch all the records of type T from the database
        /// asynchronously.
        /// </summary>
        /// <remarks>
        /// Could be costly with a large amount of records.
        /// </remarks>
        public async Task<List<T>> FetchAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Saves an entity of type T to the database.
        /// </summary>
        public void Save(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Saves an entity of type T to the database
        /// asynchronously.
        /// </summary>
        public async Task SaveAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
