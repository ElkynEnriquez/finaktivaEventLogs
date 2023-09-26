using System.Collections.Generic;

namespace EmailMessage.Models
{
    public class EmailNotificationDto
    {
        public string To { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string ContentHtml { get; set; }
        public List<EventICalendarDto> Events { get; set; } = new ();
    }
}
