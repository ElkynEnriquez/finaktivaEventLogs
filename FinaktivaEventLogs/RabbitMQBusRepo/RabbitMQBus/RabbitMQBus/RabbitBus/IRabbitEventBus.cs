using RabbitMQBus.Commands;
using RabbitMQBus.Events;
using System.Threading.Tasks;

namespace RabbitMQBus.RabbitBus
{
    public interface IRabbitEventBus
    {
        Task SendCommand<T>(T command) where T : Command;

        void Publish<T>(T @eventM) where T : Event;

        void Subscribe<T, TH>() where T : Event
                                where TH : IEventHandler<T>;

    }
}
