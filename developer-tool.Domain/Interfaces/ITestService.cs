using System.Collections.Generic;
using Infrastructure.Models;

namespace Domain.Interfaces
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