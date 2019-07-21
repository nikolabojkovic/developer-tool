using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.Interfaces;
using Application.Interfaces;
using Domain.Models;
using Domain.PersistenceModels;
using System.Threading.Tasks;
using System;

namespace Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly IRepository<TodoModel> _todoRepository;
        private readonly IMapper _mapper;
        private readonly ICacheProvider _cache;

        public TodoService(
            IRepository<TodoModel> todoRepository,
            IMapper mapper,
            ICacheProvider cache
        )
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public void Archive(int id)
        {
            var repoModel = _todoRepository.Find(x => x.Id == id)
                                           .FirstOrDefault();
            var domainModel = _mapper.Map<Todo>(repoModel);
            domainModel.Archive();
            repoModel = _mapper.Map<Todo, TodoModel>(domainModel, repoModel);
            _todoRepository.Update(repoModel);
        }

        public void Complete(int id)
        {
            var repoModel = _todoRepository.Find(x => x.Id == id)
                                           .FirstOrDefault();
            var domainModel = _mapper.Map<Todo>(repoModel);
            domainModel.Complete();
            repoModel = _mapper.Map<Todo, TodoModel>(domainModel, repoModel);
            _todoRepository.Update(repoModel);
        }

        public IEnumerable<Todo> GetAllData()
        {
            var repoModels = _todoRepository.FindAll();
            var domainModels = _mapper.Map<IEnumerable<Todo>>(repoModels);
            return domainModels;
        }

        public Todo GetById(int id)
        {
            var cachedItem = Task.Run(() => _cache.GetAsync<Todo>($"Todo_{id}")).Result;
            if (cachedItem != null)
                return cachedItem;

            var repoModel = _todoRepository.Find(x => x.Id == id)
                                           .FirstOrDefault();
            var domainModel = _mapper.Map<Todo>(repoModel);
            Task.Run(() => _cache.SetAsync<Todo>($"Todo_{id}", domainModel, TimeSpan.FromMilliseconds(_cache.CacheOptions.DefaultCacheTime))).Wait();
            return domainModel;
        }

        public void Remove(int id)
        {
            var existingItem = _todoRepository.Find(x => x.Id == id)
                                              .FirstOrDefault();
            _todoRepository.Delete(existingItem);
        }

        public void Store(Todo item)
        {
            var repoModel = _mapper.Map<TodoModel>(item);
            _todoRepository.Add(repoModel);
        }

        public void UnComplete(int id)
        {
            var repoModel = _todoRepository.Find(x => x.Id == id)
                                           .FirstOrDefault();
            var domainModel = _mapper.Map<Todo>(repoModel);
            domainModel.UnComplete();
            repoModel = _mapper.Map<Todo, TodoModel>(domainModel, repoModel);
            _todoRepository.Update(repoModel);
        }

        public void Update(Todo item)
        {
            var existingItem = _todoRepository.Find(x => x.Id == item.Id)
                                              .FirstOrDefault();            
            existingItem = _mapper.Map<Todo, TodoModel>(item, existingItem);
            _todoRepository.Update(existingItem);
        }
    }
}