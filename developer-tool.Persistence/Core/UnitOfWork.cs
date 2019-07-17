using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Infrastructure.DbContexts;

namespace Infrastructure.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private BackOfficeContext _context;

        public UnitOfWork(BackOfficeContext context)
        {
            _context = context;
        }

        public DbSet<T> Data<T>() where T : class
        {
            return _context.Set<T>(); 
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}