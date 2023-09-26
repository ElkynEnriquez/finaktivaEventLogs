using RabbitMQBus.Events;

namespace RabbitMQBus.EventQueue
{
    public class IdentityEventQueue : Event
    {
        public class User 
        { 
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string FirstSurname { get; set; }
            public string SecondSurName { get; set; }
            public int IdentificationType { get; set; }
            public string NumberIdentification { get; set; }
            public string Email { get; set; }
        }

        public User UserContent { get; set; }

        public IdentityEventQueue(User user)
        {
            UserContent = user;
        }

    }
}
