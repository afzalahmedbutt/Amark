using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TradingPortal.Core
{
    public partial interface IRepository<T> where T : BaseEntity
    {
        T Add(T t);
        Task<T> AddAsyn(T t);
        int Count();
        Task<int> CountAsync();
        void Delete(T entity);
        Task<int> DeleteAsyn(T entity);
        void DeleteRange(List<T> entities);
        Task DeleteRangeAsync(List<T> entities);
        void Dispose();
        T Find(Expression<Func<T, bool>> match);
        T Find(object id);
        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate);
        T GetById(int id);
        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(int id);
        void Save();
        Task<int> SaveAsync();
        T Update(T t, object key);
        Task<T> UpdateAsyn(T t, object key);
        Type GetEntityClrType(Type type);
        //T GetById(object id);
        //void Insert(T entity);
        //void Update(T entity);
        //void Delete(T entity);
        //IQueryable<T> Table { get; }
    }
}
