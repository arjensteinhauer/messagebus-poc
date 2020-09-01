using GreenPipes;
using MassTransit;
using MassTransit.EndpointConfigurators;
using MassTransit.Topology;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MB.Utilities.MessageBus
{
    public class MessageBusControl : IMessageBusControl
    {
        private readonly IBusControl _busControl;

        private readonly Dictionary<string, HostReceiveEndpointHandle> _receiveEndpointHandles = new Dictionary<string, HostReceiveEndpointHandle>();

        public Uri Address => _busControl.Address;

        public IBusTopology Topology => _busControl.Topology;

        public MessageBusControl(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public ConnectHandle ConnectConsumeMessageObserver<T>(IConsumeMessageObserver<T> observer) where T : class
        {
            return _busControl.ConnectConsumeMessageObserver(observer);
        }

        public ConnectHandle ConnectConsumeObserver(IConsumeObserver observer)
        {
            return _busControl.ConnectConsumeObserver(observer);
        }

        public ConnectHandle ConnectConsumePipe<T>(IPipe<ConsumeContext<T>> pipe) where T : class
        {
            return _busControl.ConnectConsumePipe(pipe);
        }

        public ConnectHandle ConnectEndpointConfigurationObserver(IEndpointConfigurationObserver observer)
        {
            return _busControl.ConnectEndpointConfigurationObserver(observer);
        }

        public ConnectHandle ConnectPublishObserver(IPublishObserver observer)
        {
            return _busControl.ConnectPublishObserver(observer);
        }

        public HostReceiveEndpointHandle ConnectReceiveEndpoint(IEndpointDefinition definition, IEndpointNameFormatter endpointNameFormatter, Action<IReceiveEndpointConfigurator> configureEndpoint = null)
        {
            return _busControl.ConnectReceiveEndpoint(definition, endpointNameFormatter, configureEndpoint);
        }

        public HostReceiveEndpointHandle ConnectReceiveEndpoint(string queueName, Action<IReceiveEndpointConfigurator> configureEndpoint)
        {
            if (_receiveEndpointHandles.ContainsKey(queueName))
            {
                throw new InstanceAlreadySubscribedToBusException(queueName);
            }

            var receiveEndpoint = _busControl.ConnectReceiveEndpoint(queueName, configureEndpoint);
            _receiveEndpointHandles.Add(queueName, receiveEndpoint);
            return receiveEndpoint;
        }

        public async Task DisconnectReceiveEndpoint(string queueName)
        {
            if (!_receiveEndpointHandles.ContainsKey(queueName))
            {
                return;
            }

            var handle = _receiveEndpointHandles[queueName];
            await handle.StopAsync().ConfigureAwait(false);

            _receiveEndpointHandles.Remove(queueName);
        }

        public ConnectHandle ConnectReceiveEndpointObserver(IReceiveEndpointObserver observer)
        {
            return _busControl.ConnectReceiveEndpointObserver(observer);
        }

        public ConnectHandle ConnectReceiveObserver(IReceiveObserver observer)
        {
            return _busControl.ConnectReceiveObserver(observer);
        }

        public ConnectHandle ConnectRequestPipe<T>(Guid requestId, IPipe<ConsumeContext<T>> pipe) where T : class
        {
            return _busControl.ConnectRequestPipe(requestId, pipe);
        }

        public ConnectHandle ConnectSendObserver(ISendObserver observer)
        {
            return _busControl.ConnectSendObserver(observer);
        }

        public Task<ISendEndpoint> GetPublishSendEndpoint<T>() where T : class
        {
            return _busControl.GetPublishSendEndpoint<T>();
        }

        public Task<ISendEndpoint> GetSendEndpoint(Uri address)
        {
            return _busControl.GetSendEndpoint(address);
        }

        public void Probe(ProbeContext context)
        {
            _busControl.Probe(context);
        }

        public Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            return _busControl.Publish(message, cancellationToken);
        }

        public Task Publish<T>(T message, IPipe<PublishContext<T>> publishPipe, CancellationToken cancellationToken = default) where T : class
        {
            return _busControl.Publish(message, publishPipe, cancellationToken);
        }

        public Task Publish<T>(T message, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default) where T : class
        {
            return _busControl.Publish(message, publishPipe, cancellationToken);
        }

        public Task Publish(object message, CancellationToken cancellationToken = default)
        {
            return _busControl.Publish(message, cancellationToken);
        }

        public Task Publish(object message, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default)
        {
            return _busControl.Publish(message, publishPipe, cancellationToken);
        }

        public Task Publish(object message, Type messageType, CancellationToken cancellationToken = default)
        {
            return _busControl.Publish(message, messageType, cancellationToken);
        }

        public Task Publish(object message, Type messageType, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default)
        {
            return _busControl.Publish(message, messageType, publishPipe, cancellationToken);
        }

        public Task Publish<T>(object values, CancellationToken cancellationToken = default) where T : class
        {
            return _busControl.Publish<T>(values, cancellationToken);
        }

        public Task Publish<T>(object values, IPipe<PublishContext<T>> publishPipe, CancellationToken cancellationToken = default) where T : class
        {
            return _busControl.Publish(values, publishPipe, cancellationToken);
        }

        public Task Publish<T>(object values, IPipe<PublishContext> publishPipe, CancellationToken cancellationToken = default) where T : class
        {
            return _busControl.Publish<T>(values, publishPipe, cancellationToken);
        }

        public Task<BusHandle> StartAsync(CancellationToken cancellationToken = default)
        {
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return _busControl.StopAsync(cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            foreach (var receiveEndpointHandle in _receiveEndpointHandles)
            {
                receiveEndpointHandle.Value.StopAsync().Wait();
            }

            _receiveEndpointHandles.Clear();
        }
    }
}
