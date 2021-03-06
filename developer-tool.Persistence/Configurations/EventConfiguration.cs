using Domain.PersistenceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<CalendarEventModel>
    {
        public void Configure(EntityTypeBuilder<CalendarEventModel> builder)
        {
            builder.ToTable("CalendarEvents");
            builder.HasKey(x => x.Id)
                   .HasAnnotation("MySql:ValueGeneratedOnAdd", true);
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Start).IsRequired();
        }
    }
}