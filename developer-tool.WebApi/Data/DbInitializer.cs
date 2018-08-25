using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TestContext context)
        {
            context.Database.Migrate();
            context.Database.EnsureCreated();

            if (context.TestItems.Any())
            {
                return; // Db has been seeded;
            }

            var items = new Test[]
            {
                new Test { FirstName = "Test 1" },
                new Test { FirstName = "Test 2" },
                new Test { FirstName = "Test 3" }
            };

            context.TestItems.AddRange(items);
            context.SaveChanges();
        }
    }
}