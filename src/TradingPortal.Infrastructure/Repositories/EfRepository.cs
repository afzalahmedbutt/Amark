using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TradingPortal.Core;
using TradingPortal.Infrastructure.DatabaseContexts;

namespace TradingPortal.Infrastructure.Repositories
{
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _entities;

        public EfRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {

            return await _context.Set<T>().ToListAsync();
        }

        public virtual T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual T Add(T t)
        {

            _context.Set<T>().Add(t);
            //_context.SaveChanges();
            return t;
        }

        public virtual async Task<T> AddAsyn(T t)
        {
            _context.Set<T>().Add(t);
            await _context.SaveChangesAsync();
            return t;

        }

        public virtual T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().SingleOrDefault(match);
        }

        public T Find(object id)
        {
            return this.Entities.Find(id);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Where(match).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().Where(match).ToListAsync();
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            
        }

        public virtual void DeleteRange(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            
        }

        public virtual async Task DeleteRangeAsync(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsyn(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public virtual T Update(T t, object key)
        {
            if (t == null)
                return null;
            T exist = _context.Set<T>().Find(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                //_context.SaveChanges();
            }
            return exist;
        }

        public virtual async Task<T> UpdateAsyn(T t, object key)
        {
            if (t == null)
                return null;
            T exist = await _context.Set<T>().FindAsync(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                await _context.SaveChangesAsync();
            }
            return exist;
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public virtual void Save()
        {

            _context.SaveChanges();
        }

        public async virtual Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            return query;
        }

        public virtual async Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        private DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        public Type GetEntityClrType(Type type)
        {
            return _context.Model.FindEntityType(type).ClrType;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

