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
        public string Title { get; set; }
        public string Greeting { get; set; }
        public string Description { get; set; }
        public string ContentHtml { get; set; }
        public int Type { get; set; }
        public int LogoHeader { get; set; }
        public List<string> HtmlActions { get; set; }
        public List<string> Images64 { get; set; }
        public List<EventICalendarDto> Events { get; set; }

        public NotificationEmailEventQueue(string to, string displayName, string subject, string title, string greeting, string description, string contentHtml, int type, int logoHeader, List<string> htmlActions, List<string> images64, List<EventICalendarDto> events)
        {
            To = to;
            DisplayName = displayName;
            Subject = subject;
            Title = title;
            Greeting = greeting;
            Description = description;
            ContentHtml = contentHtml;
            Type = type;
            LogoHeader = logoHeader;
            HtmlActions = htmlActions;
            Images64 = images64;
            Events = events;
        }
    }
}
