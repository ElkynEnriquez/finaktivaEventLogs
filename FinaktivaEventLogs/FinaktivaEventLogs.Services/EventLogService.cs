using AutoMapper;
using FinaktivaEventLogs.Domain.Common;
using FinaktivaEventLogs.Domain.Entities;
using FinaktivaEventLogs.Domain.Interfaces;
using FinaktivaEventLogs.Services.Common;
using FinaktivaEventLogs.Services.DTO;
using FinaktivaEventLogs.Services.DTO.Common;
using FinaktivaEventLogs.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FinaktivaEventLogs.Services
{
    public class EventLogService : BaseService, IEventLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EventLogService(IMapper mapper, ILoggerManager loggerManager, IHttpContextAccessor httpContext, IUnitOfWork unitOfWork) : base(mapper, loggerManager, httpContext)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse<EventLogDto>> CreateAsync(EventLogCreateDto AwardCreate)
        {
            try
            {
                EventLog EventLogEntity = _mapper.Map<EventLog>(AwardCreate);

                if (EventLogEntity == null)
                    throw new ArgumentException("EventLog or a required property is null");

                EventLogEntity.CreateRegisterDate = DateTime.Now;

                _unitOfWork.BeginTransaction();
                _unitOfWork.EventLogRepository.Create(EventLogEntity);
                await _unitOfWork.CommitAsync();

                return new ServiceResponse<EventLogDto>(_mapper.Map<EventLogDto>(EventLogEntity))
                { Message = "Datos insertados correctamente" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in EventLogService::CreateAsync:: {ex.Message} ");
                throw;
            }
        }

        public async Task<ServiceResponse<PaginationDto<EventLogDto>>> GetWithPaginationAsync(RequestPaginationDto pagination)
        {
            try
            {
                // Se crea el objeto para consulta
                Pagination<EventLog>? paginationEntity = new Pagination<EventLog>()
                {
                    PageSize = pagination.PageSize,
                    Page = pagination.Page,
                    Sort = pagination.Sort,
                    SortDirection = pagination.SortDirection,
                };
                // Si el filtro existe, se añade al objeto de consulta
                if (pagination.FilterValue != null)
                    paginationEntity.FilterValue = _mapper.Map<FilterValue>(pagination.FilterValue);
                // Consulta a base de datos, con el objeto paginación
                var resultPaginationEntity = await _unitOfWork.EventLogRepository.PaginationByFilter(paginationEntity);
                List<EventLogDto>? eventLogsDto = new();
                if (resultPaginationEntity.Data != null)
                    eventLogsDto = _mapper.Map<List<EventLogDto>>(resultPaginationEntity.Data);

                // Se crea Objeto para respuesta
                PaginationDto<EventLogDto>? paginationResponseDto = new ()
                {
                    Data = eventLogsDto,
                    PageSize = resultPaginationEntity.PageSize,
                    PagesQuantity = resultPaginationEntity.PagesQuantity,
                    TotalRows = resultPaginationEntity.TotalRows,
                    Page = resultPaginationEntity.Page,
                    Sort = resultPaginationEntity.Sort,
                    SortDirection = resultPaginationEntity.SortDirection,
                };

                return new ServiceResponse<PaginationDto<EventLogDto>>(paginationResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en EventLogService::GetWithPaginationAsync:: {ex.Message}");
                return new ServiceResponse<PaginationDto<EventLogDto>>($"EventLogService::GetWithPaginationAsync:: {ex.Message}");
            }
        }
    }
}