using Microsoft.EntityFrameworkCore;
using Domain.PersistenceModels;

namespace Infrastructure.DbContexts
{
    public class InMemoryContext : DbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options)
            :base(options)
        {
        }

        public DbSet<TodoModel> TodoItems { get; set; }
    }
}