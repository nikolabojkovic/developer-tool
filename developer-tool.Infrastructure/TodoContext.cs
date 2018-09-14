using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;

namespace Infrastructure
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            :base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}