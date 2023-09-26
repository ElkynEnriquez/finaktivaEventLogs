using RabbitMQBus.Events;

namespace RabbitMQBus.EventQueue
{
    public class IdentityEmailEventQueue : Event
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public IdentityEmailEventQueue(string to, string subject, string content, int type)
        {
            To = to;
            Subject = subject;
            Content = content;
            Type = type;
        }

    }
}
