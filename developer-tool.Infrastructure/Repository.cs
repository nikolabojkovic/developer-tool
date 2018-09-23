using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Interfaces;

namespace Infrastructure
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<T> FindAll()
        {
            return _unitOfWork.Data.Set<T>();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Data.Set<T>().Where(predicate);
        } 

        public void Add(T newEntity)
        {
            _unitOfWork.Data.Set<T>().Add(newEntity);
        }

        public void Update(T entity)
        {
            _unitOfWork.Data.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _unitOfWork.Data.Set<T>().Remove(entity);
        }
    }
}