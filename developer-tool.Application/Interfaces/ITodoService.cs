using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ITodoService
    {
        IEnumerable<Todo> GetAllData();
        Task<Todo> GetByIdAsync(int id);
        void Store(Todo item);
        void Update(Todo item);
        void Remove(int id);
        void Complete(int id);
        void UnComplete(int id);
        void Archive(int id);
    }
}