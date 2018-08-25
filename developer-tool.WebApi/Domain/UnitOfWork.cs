using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private TestContext _context;

        public UnitOfWork(TestContext context)
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