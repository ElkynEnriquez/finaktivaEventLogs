using AutoMapper;
using EmailMessage.Models;
using EmailMessage.Services.Contract;
using RabbitMQBus.EventQueue;
using RabbitMQBus.RabbitBus;

namespace FinaktivaEventLogs.API.RabbitHandler
{
    public class NotificationEmailEventHandler : IEventHandler<NotificationEmailEventQueue>
    {

        private readonly ILogger<NotificationEmailEventHandler> _logger;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public NotificationEmailEventHandler(ILogger<NotificationEmailEventHandler> logger, IEmailService emailService, IMapper mapper)
        {
            _logger = logger;
            _emailService = emailService;
            _mapper = mapper;
        }
        public Task Handle(NotificationEmailEventQueue @event)
        {
            _logger.LogInformation(@event.ContentHtml);
            EmailNotificationDto emailData = new()
            {
                To = @event.To,
                DisplayName = @event.DisplayName,
                Subject = @event.Subject,
                ContentHtml = @event.ContentHtml,
                Events = _mapper.Map<List<EmailMessage.Models.EventICalendarDto>>(@event.Events),
            };

            _emailService.SendNotification(emailData);
            return Task.CompletedTask;
        }
    }
}
