using MassTransit;
using System;
using System.Threading.Tasks;

namespace MB.Utilities.MessageBus
{
    public class EventSubscriber : IEventSubscriber
    {
        private readonly IMessageBusControl _busControl;

        public EventSubscriber(IMessageBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task Subscribe<TEvent>(string handlerName, Type consumerType, Func<Type, object> consumerFactory) where TEvent : class
        {
            string queueName = $"{handlerName}_{typeof(TEvent).FullName}";

            var receiveEndpoint = _busControl.ConnectReceiveEndpoint(queueName, config =>
            {
                config.Consumer(consumerType, consumerFactory);
            });

            await receiveEndpoint.Ready;
        }

        public async Task Unsubscribe<TEvent>(string handlerName)
        {
            string queueName = $"{handlerName}_{typeof(TEvent).FullName}";

            await _busControl.DisconnectReceiveEndpoint(queueName).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _busControl?.Dispose();
        }
    }
}
