using RabbitMQBus.Events;
using System.Threading.Tasks;

namespace RabbitMQBus.RabbitBus
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {

    }
}
