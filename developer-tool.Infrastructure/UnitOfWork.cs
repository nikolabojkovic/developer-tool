using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private BackOfficeContext _context;

        public UnitOfWork(BackOfficeContext context)
        {
            _context = context;
        }

        public DbContext Data
        {
            get { return _context; }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}