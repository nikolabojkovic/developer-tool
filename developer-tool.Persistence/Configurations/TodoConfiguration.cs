using Domain.PersistenceModels;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class TodoConfiguration : IEntityTypeConfiguration<TodoModel>
    {
        public void Configure(EntityTypeBuilder<TodoModel> builder)
        {
            builder.ToTable("Todos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description).IsRequired();
        }
    }
}