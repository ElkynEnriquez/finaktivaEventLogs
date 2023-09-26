using FinaktivaEventLogs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FinaktivaEventLogs.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<EventLog>? EventLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(mb);
        }
    }
}
