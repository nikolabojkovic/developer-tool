using System.Collections.Generic;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ITodoService
    {
        IEnumerable<Todo> GetAllData();
        Todo GetById(int id);
        void Store(Todo item);
        void Update(Todo item);
        void Remove(int id);
        void Complete(int id);
        void UnComplete(int id);
        void Archive(int id);
    }
}