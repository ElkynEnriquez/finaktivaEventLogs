using AutoMapper;
using FinaktivaEventLogs.Services.Common;
using Microsoft.AspNetCore.Http;

namespace FinaktivaEventLogs.Services
{
    public class BaseService
    {
        protected readonly IMapper _mapper;
        protected readonly ILoggerManager _logger;
        protected readonly IHttpContextAccessor _httpContext;
        public BaseService(IMapper mapper, ILoggerManager loggerManager, IHttpContextAccessor httpContext)
        {
            _mapper = mapper;
            _logger = loggerManager;
            _httpContext = httpContext;
        }
    }
}
