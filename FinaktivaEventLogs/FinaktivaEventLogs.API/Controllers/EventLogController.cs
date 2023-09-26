using FinaktivaEventLogs.API.Extensions;
using FinaktivaEventLogs.Services;
using FinaktivaEventLogs.Services.DTO;
using FinaktivaEventLogs.Services.DTO.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinaktivaEventLogs.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class EventLogController : Controller
    {
        private readonly EventLogService _eventLogService;
        public EventLogController(EventLogService eventLogService)
        {
            _eventLogService = eventLogService; ;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] EventLogCreateDto eventLog)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _eventLogService.CreateAsync(eventLog);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
        //POST: Get with pagination
        [HttpPost("Pagination")]
        public async Task<ActionResult<PaginationDto<EventLogDto>>> GetWithPaginationAsync(RequestPaginationDto pagination)
        {
            var result = await _eventLogService.GetWithPaginationAsync(pagination);
            return Ok(result);
        }
    }
}
