using AutoMapper;
using FinaktivaEventLogs.Domain.Common;
using FinaktivaEventLogs.Services.DTO.Common;

namespace FinaktivaEventLogs.Services.AutoMapper
{
    public class FilterValueProfile : Profile
    {
        public FilterValueProfile()
        {
            CreateMap<FilterValue, FilterValueDto>().ReverseMap();
        }
    }
}
