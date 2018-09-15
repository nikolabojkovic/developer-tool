using Microsoft.EntityFrameworkCore;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        DbContext Data { get; }
        void Commit();
    }
}