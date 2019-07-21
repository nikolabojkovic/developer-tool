using Microsoft.EntityFrameworkCore;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        DbSet<T> Data<T>() where T : class;
        void Commit();
    }
}