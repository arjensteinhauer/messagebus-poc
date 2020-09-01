using MB.Manager.Message1.Interface.V1;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace MB.Client.Desktop.App.EventHandlers
{
    public class Message1EventHandler : IMessage1EventHandler
    {
        /// <summary>
        /// SignalR hub connection.
        /// </summary>
        private readonly HubConnection _connection;

        /// <summary>
        /// Event handler for handling OnEchoed events.
        /// </summary>
        public event EventHandler<EchoedEventData> OnEchoed;

        /// <summary>
        /// Event handler for handling OnProcessedOneWayCommand events.
        /// </summary>
        public event EventHandler<ProcessedOneWayCommandEventData> OnProcessedOneWayCommand;

        /// <summary>
        /// Event handler for handling OnAmAlive events.
        /// </summary>
        public event EventHandler<AmAliveEventData> OnAmAlive;

        /// <summary>
        /// Event handler for handling OnPublishSomething events.
        /// </summary>
        public event EventHandler<PublishSomethingEventData> OnPublishSomething;

        /// <summary>
        /// SignalR connection ID.
        /// </summary>
        public string ConnectionId { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="connection"></param>
        public Message1EventHandler(HubConnection connection)
        {
            _connection = connection;
            _connection.Closed += (ex) =>
            {
                // on disconnect, remove the current event handlers
                _connection.Remove("OnEchoed");
                _connection.Remove("OnProcessedOneWayCommand");
                _connection.Remove("OnAmAlive");
                _connection.Remove("OnPublishSomething");
                return Task.FromResult(true);
            };
            _connection.Reconnected += async (connectionId) =>
            {
                // save the new connection ID
                ConnectionId = connectionId;

                // on reconnect, subscribe on the events
                await SubscribeOnEvents().ConfigureAwait(false);
            };
        }

        /// <summary>
        /// Subscribe on OnEchoed events.
        /// </summary>
        public async Task SubscribeOnEvents()
        {
            _connection.On<EchoedEventData>("OnEchoed", (eventData) =>
            {
                OnEchoed?.Invoke(this, eventData);
            });
            _connection.On<ProcessedOneWayCommandEventData>("OnProcessedOneWayCommand", (eventData) =>
            {
                OnProcessedOneWayCommand?.Invoke(this, eventData);
            });
            _connection.On<AmAliveEventData>("OnAmAlive", (eventData) =>
            {
                OnAmAlive?.Invoke(this, eventData);
            });
            _connection.On<PublishSomethingEventData>("OnPublishSomething", (eventData) =>
            {
                OnPublishSomething?.Invoke(this, eventData);
            });

            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync().ConfigureAwait(false);

                ConnectionId = _connection.ConnectionId;
            }
        }

        /// <summary>
        /// Subscribe on events for a specific name.
        /// </summary>
        /// <param name="subscriptionName">Name of the </param>
        public async Task SubscribeOnEventsFor(string subscriptionName)
        {
            if (_connection.State != HubConnectionState.Connected)
            {
                throw new Exception("Not connected through SignalR");
            }

            await _connection.InvokeAsync("Subscribe", subscriptionName);
        }

        /// <summary>
        /// Unsubscribe on events for a specific name.
        /// </summary>
        /// <param name="subscriptionName">Name of the </param>
        public async Task UnsubscribeOnEventsFor(string subscriptionName)
        {
            if (_connection.State != HubConnectionState.Connected)
            {
                throw new Exception("Not connected through SignalR");
            }

            await _connection.InvokeAsync("Unsubscribe", subscriptionName);
        }

        /// <summary>
        /// Disconnect the signalR connection.
        /// </summary>
        /// <returns>Async task.</returns>
        public async Task Disconnect()
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.StopAsync().ConfigureAwait(false);
            }
        }
    }
}
