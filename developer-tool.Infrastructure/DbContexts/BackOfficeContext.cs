using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;

namespace Infrastructure.DbContexts
{
    public class BackOfficeContext : DbContext
    {
        public BackOfficeContext(DbContextOptions<BackOfficeContext> options)
            :base(options)
        {
        }

        public DbSet<Event> CalendarEvents { get; set; }
    }
}