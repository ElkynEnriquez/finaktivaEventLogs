using FinaktivaEventLogs.Services.DTO.Common;
using FinaktivaEventLogs.Services.DTO.Enums;

namespace FinaktivaEventLogs.Services.DTO
{
    public class EventLogDto : AuditableDto
    {
        public string Title { get; set; }
        public EEventTypeDto Type { get; set; }
        public string Description { get; set; }
    }
}
