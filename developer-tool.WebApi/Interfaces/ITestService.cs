using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ITestService
    {
        IEnumerable<Test> GetAllData();
        Test GetById(int id);
        void Store(Test entity);
        void Update(Test entity);
        void Remove(Test entity);
    }
}