using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using Infrastructure.Configurations;
using System.Reflection;
using System.Linq;
using Infrastructure.Core;
using Infrastructure.Extensions;

namespace Infrastructure.DbContexts
{
    public class BackOfficeContext : DbContext
    {
        public BackOfficeContext(DbContextOptions<BackOfficeContext> options)
            :base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEntityTypeConfiguration(typeof(TodoConfiguration).Assembly);

            RegisterEntities(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void RegisterEntities(ModelBuilder modelBuilder)
        {
            MethodInfo entityMethod = typeof(ModelBuilder).GetMethods().First(m => m.Name == "Entity" && m.IsGenericMethod);

            var entityTypes = Assembly.GetAssembly(typeof(TodoModel)).GetTypes()
                .Where(x => x.IsSubclassOf(typeof(Entity)) && !x.IsAbstract);

            foreach (var type in entityTypes)
            {
                entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}