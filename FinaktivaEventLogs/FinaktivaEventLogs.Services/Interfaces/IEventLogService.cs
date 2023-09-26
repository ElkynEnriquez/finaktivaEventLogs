using FinaktivaEventLogs.Services.Common;
using FinaktivaEventLogs.Services.DTO;
using FinaktivaEventLogs.Services.DTO.Common;

namespace FinaktivaEventLogs.Services.Interfaces
{
    public interface IEventLogService
    {
        Task<ServiceResponse<EventLogDto>> CreateAsync(EventLogCreateDto eventLogCreate);
        Task<ServiceResponse<PaginationDto<EventLogDto>>> GetWithPaginationAsync(RequestPaginationDto pagination);
    }
}
