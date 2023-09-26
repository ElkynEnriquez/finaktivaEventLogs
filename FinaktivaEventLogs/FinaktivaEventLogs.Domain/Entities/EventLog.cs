using FinaktivaEventLogs.Domain.Common;
using FinaktivaEventLogs.Domain.Entities.Enums;

namespace FinaktivaEventLogs.Domain.Entities
{
    public class EventLog : Auditable
    {
        public EEventType Type { get; set; }
        public string Description { get; set; }
    }
}
