using System;
using Domain.PersistenceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Configurations
{
    public class TodoConfiguration : IEntityTypeConfiguration<TodoModel>
    {
        public void Configure(EntityTypeBuilder<TodoModel> builder)
        {
            builder.ToTable("Todos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.IsCompleted);
            builder.Property(x => x.IsArchived);
        }
    }
}