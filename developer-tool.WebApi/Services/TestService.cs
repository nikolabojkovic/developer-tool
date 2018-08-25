using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Interfaces;
using WebApi.Domain;
using WebApi.Models;

namespace WebApi.Services
{
    public class TestService : ITestService
    {
        private readonly IRepository<Test> _testRepository;
        
        public TestService(IRepository<Test> testRepository)
        {
            _testRepository = testRepository;
        }

        public IEnumerable<Test> GetAllData()
        {
            return _testRepository.FindAll();
        }

        public Test GetById(int id)
        {
            return _testRepository.Find(x => x.Id == id).FirstOrDefault();
        }

        public void Store(Test entity)
        {
            _testRepository.Add(entity);
        }

        public void Update(Test entity)
        {
            var existingItem = GetById(entity.Id);
            existingItem.FirstName = entity.FirstName;
            
            _testRepository.Update(existingItem);
        }

        public void Remove(Test entity)
        {
            _testRepository.Delete(entity);
        }
    }
}