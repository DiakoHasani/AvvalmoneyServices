using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AS.DAL.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        DataContext _context;
        DbSet<T> _dbSet;

        public BaseRepository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet?.Remove(entity);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> where = null)
        {
            var data = (IQueryable<T>)_dbSet;
            if (where != null)
            {
                data = data.Where(where);
            }

            return data;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
        IQueryable<T> GetAll(Expression<Func<T, bool>> where = null);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangeAsync();
    }
}
