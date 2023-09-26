#nullable enable
using System;

namespace EmailMessage.Models
{
    public class EventICalendarDto
    {
        public string EmailOrganizer { get; set; }
        public string NameOrganizer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Summary { get; set; }
        public string? Description { get; set; }

    }
}
