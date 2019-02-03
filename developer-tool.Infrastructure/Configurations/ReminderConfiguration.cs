using Infrastructure.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RemiderConfiguration : IEntityTypeConfiguration<ReminderModel>
    {
        public void Configure(EntityTypeBuilder<ReminderModel> builder)
        {
            builder.ToTable("Reminders");
            builder.HasKey(x => x.Id);
        }
    }
}