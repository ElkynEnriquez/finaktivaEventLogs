using AutoMapper;
using FinaktivaEventLogs.Domain.Entities;
using FinaktivaEventLogs.Services.DTO;

namespace FinaktivaEventLogs.Services.AutoMapper
{
    public class EventLogProfile : Profile
    {
        public EventLogProfile()
        {
            CreateMap<EventLog, EventLogDto>().ReverseMap();
            CreateMap<EventLog, EventLogCreateDto>().ReverseMap();
        }
    }
}
