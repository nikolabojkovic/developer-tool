using Domain.PersistenceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class RemiderConfiguration : IEntityTypeConfiguration<ReminderModel>
    {
        public void Configure(EntityTypeBuilder<ReminderModel> builder)
        {
            builder.ToTable("Reminders");
            builder.Property(x => x.Active);
            builder.HasKey(x => x.Id)
                   .HasAnnotation("MySql:ValueGeneratedOnAdd", true);
            builder.HasOne(x => x.Event)
                   .WithMany(x => x.Reminders)
                   .HasForeignKey(x => x.EventId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}