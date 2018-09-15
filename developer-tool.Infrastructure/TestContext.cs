using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;

namespace Infrastructure
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options)
            :base(options)
        {
        }

        public DbSet<Test> TestItems { get; set; }
        public DbSet<Test2> Test2Items { get; set; }
    }
}