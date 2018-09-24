using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;

namespace Infrastructure
{
    public class BackOfficeContext : DbContext
    {
        public BackOfficeContext(DbContextOptions<BackOfficeContext> options)
            :base(options)
        {
        }

        public DbSet<CalendarEvent> CalendarEvents { get; set; }
    }
}