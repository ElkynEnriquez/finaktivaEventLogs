using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQBus.Commands;
using RabbitMQBus.Events;
using RabbitMQBus.RabbitBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQBus.Implement
{
    public class RabbitEventBus : IRabbitEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitEventBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
            _serviceScopeFactory = serviceScopeFactory;
        }


        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Publish<T>(T eventM) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "votomaster-rabbit-web" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = eventM.GetType().Name;

                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(eventM);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var eventTypesHandler = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(x => x.GetType() == eventTypesHandler))
            {
                throw new ArgumentException($"El manejador {eventTypesHandler.Name} fue registrado anteriormente por {eventName}");
            }

            _handlers[eventName].Add(eventTypesHandler);

            var factory = new ConnectionFactory()
            {
                HostName = "votomaster-rabbit-web",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();


            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Delegate;

            channel.BasicConsume(eventName, true, consumer);

        }

        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                if (_handlers.ContainsKey(eventName))
                {
                    using (var scope = _serviceScopeFactory.CreateScope()) {

                        var subscriptions = _handlers[eventName];
                        foreach (var sb in subscriptions)
                        {
                            var handler = scope.ServiceProvider.GetService(sb); //Activator.CreateInstance(sb);
                            if (handler == null) continue;

                            var eventType = _eventTypes.SingleOrDefault(x => x.Name == eventName);
                            var eventDS = JsonConvert.DeserializeObject(message, eventType);

                            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { eventDS });

                        }

                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
