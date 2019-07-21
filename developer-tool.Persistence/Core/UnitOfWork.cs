using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Persistence.DbContexts;

namespace Persistence.Core
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