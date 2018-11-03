using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Core
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private IUnitOfWork _unitOfWork;
        private ILogger _logger;

        public Repository(
            IUnitOfWork unitOfWork,
            ILogger<Repository<T>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IQueryable<T> FindAll()
        {
            _logger.LogInformation("Getting all items");
            return _unitOfWork.Data.Set<T>();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            _logger.LogInformation("Getting item by condition");
            return _unitOfWork.Data.Set<T>().Where(predicate);
        } 

        public void Add(T newEntity)
        {
            _logger.LogInformation("Adding new item");
            _unitOfWork.Data.Set<T>().Add(newEntity);
        }

        public void Update(T entity)
        {
            _logger.LogInformation("Updating item with id " + entity.Id);
            _unitOfWork.Data.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _logger.LogInformation("Deleting item with id " + entity.Id);
            _unitOfWork.Data.Set<T>().Remove(entity);
        }
    }
}