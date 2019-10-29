using Microsoft.EntityFrameworkCore;
using Domain.PersistenceModels;
using Persistence.Configurations;
using System.Reflection;
using System.Linq;
using Persistence.Extensions;
using Persistence.Core;

namespace Persistence.DbContexts
{
    public class BackOfficeContext : DbContext
    {
        public BackOfficeContext(DbContextOptions<BackOfficeContext> options)
            :base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);           

            modelBuilder.UseEntityTypeConfiguration(typeof(TodoConfiguration).Assembly);

            RegisterEntities(modelBuilder);
        }

        private void RegisterEntities(ModelBuilder modelBuilder)
        {
            MethodInfo entityMethod = typeof(ModelBuilder).GetMethods().First(m => m.Name == "Entity" && m.IsGenericMethod);

            var entityTypes = Assembly.GetAssembly(typeof(TodoModel)).GetTypes()
                .Where(x => x.IsSubclassOf(typeof(EntityModel)) && !x.IsAbstract);

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