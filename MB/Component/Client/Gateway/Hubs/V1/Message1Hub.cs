using MB.Manager.Message1.Interface.V1;
using MB.Manager.Message1.Proxy.V1;
using MB.Utilities.MessageBus;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MB.Client.Gateway.Service.Hubs.V1
{
    public class Message1Hub : Hub<IMessage1ManagerEvents>
    {
        private readonly IEventSubscriber _eventSubscriber;
        private readonly IPublishSomethingEvent _eventHandler;
        private readonly ILogger _logger;

        public Message1Hub(IEventSubscriber eventSubscriber, IPublishSomethingEvent eventHandler, ILoggerFactory loggerFactory)
        {
            _eventSubscriber = eventSubscriber;
            _eventHandler = eventHandler;
            _logger = loggerFactory.CreateLogger<Message1Hub>();
        }

        public async Task Subscribe(string name)
        {
            try
            {
                _logger.LogInformation($"Subscribe to '{name}' messages for client {Context.ConnectionId}...");

                await this.Groups.AddToGroupAsync(Context.ConnectionId, name);
                var consumerType = typeof(PublishSomethingEventConsumer);
                await _eventSubscriber.Subscribe<PublishSomethingEventData>($"{name}_{Context.ConnectionId}", consumerType, _ => Activator.CreateInstance(consumerType, _eventHandler));

                _logger.LogInformation($"\t--> subscribed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during subscription to '{name}' messages for client {Context.ConnectionId}");
                throw;
            }
        }

        public async Task Unsubscribe(string name)
        {
            try
            {
                _logger.LogInformation($"Unsubscribe to '{name}' messages for client {Context.ConnectionId}");

                await _eventSubscriber.Unsubscribe<PublishSomethingEventData>($"{name}_{Context.ConnectionId}");
                await this.Groups.RemoveFromGroupAsync(Context.ConnectionId, name);

                _logger.LogInformation($"\t--> unsubscribed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during unsubscribe to '{name}' messages for client {Context.ConnectionId}");
                throw;
            }
        }
    }
}