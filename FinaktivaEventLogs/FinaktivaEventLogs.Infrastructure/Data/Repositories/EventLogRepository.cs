using FinaktivaEventLogs.Domain.Entities;
using FinaktivaEventLogs.Domain.Interfaces.Repositories;

namespace FinaktivaEventLogs.Infrastructure.Data.Repositories
{
    public class EventLogRepository : GenericRepository<EventLog>, IEventLogRepository
    {
        public EventLogRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        { }
    }
}
