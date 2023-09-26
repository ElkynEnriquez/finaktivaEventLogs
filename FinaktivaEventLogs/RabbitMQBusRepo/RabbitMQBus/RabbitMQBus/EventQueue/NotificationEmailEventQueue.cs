using System.Collections.Generic;
using RabbitMQBus.Dtos;
using RabbitMQBus.Events;

namespace RabbitMQBus.EventQueue
{
    public class NotificationEmailEventQueue : Event
    {
        public string To { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string ContentHtml { get; set; }
        public List<EventICalendarDto> Events { get; set; }

        public NotificationEmailEventQueue(string to, string displayName, string subject, string contentHtml, List<EventICalendarDto> events)
        {
            To = to;
            DisplayName = displayName;
            Subject = subject;
            ContentHtml = contentHtml;
            Events = events;
        }
    }
}
