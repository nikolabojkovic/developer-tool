using Microsoft.EntityFrameworkCore;

namespace WebApi.Interfaces
{
    public interface IUnitOfWork
    {
        DbContext Data { get; }
        void Commit();
    }
}