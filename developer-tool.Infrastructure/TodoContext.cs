using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;

namespace Infrastructure
{
    public class InMemoryContext : DbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options)
            :base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}