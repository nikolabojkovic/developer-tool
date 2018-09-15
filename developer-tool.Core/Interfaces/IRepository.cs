using System;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);      
        void Add(T newEntity);
        void Update(T entity);
        void Delete(T entity);
    }
}