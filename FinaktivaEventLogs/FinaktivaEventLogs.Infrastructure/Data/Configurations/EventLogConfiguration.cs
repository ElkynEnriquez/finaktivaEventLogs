using FinaktivaEventLogs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinaktivaEventLogs.Infrastructure.Data.Configurations
{
    public class EventLogConfiguration : IEntityTypeConfiguration<EventLog>
    {
        public void Configure(EntityTypeBuilder<EventLog> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(d => d.CreateRegisterDate).HasDefaultValueSql("getdate()");
        }
    }
}
