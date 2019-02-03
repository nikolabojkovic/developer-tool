using Infrastructure.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<EventModel>
    {
        public void Configure(EntityTypeBuilder<EventModel> builder)
        {
            builder.ToTable("CalendarEvents");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Start).IsRequired();
            builder.HasOne(x => x.Reminder)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}