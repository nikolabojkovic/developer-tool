using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
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