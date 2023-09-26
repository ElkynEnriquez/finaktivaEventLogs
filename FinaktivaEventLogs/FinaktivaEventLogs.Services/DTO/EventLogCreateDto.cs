using FinaktivaEventLogs.Services.DTO.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FinaktivaEventLogs.Services.DTO
{
    public class EventLogCreateDto
    {
        public Guid CreateRegisterByUserId { get; set; }
        public DateTime CreateRegisterDate { get; set; } = DateTime.Now;

        [JsonConverter(typeof(StringEnumConverter))]
        public EEventTypeDto Type { get; set; }
        public string Description { get; set; }
    }
}
