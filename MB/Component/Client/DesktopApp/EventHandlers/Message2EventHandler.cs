using MB.Manager.Message2.Interface.V1;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace MB.Client.Desktop.App.EventHandlers
{
    public class Message2EventHandler : IMessage2EventHandler
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
        /// Event handler for handling OnAmAlive events.
        /// </summary>
        public event EventHandler<AmAliveEventData> OnAmAlive;

        /// <summary>
        /// SignalR connection ID.
        /// </summary>
        public string ConnectionId { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="connection"></param>
        public Message2EventHandler(HubConnection connection)
        {
            _connection = connection;
            _connection.Closed += (ex) =>
            {
                // on disconnect, remove the current event handlers
                _connection.Remove("OnEchoed");
                _connection.Remove("OnAmAlive");
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
        /// Subscribe on events.
        /// </summary>
        public async Task SubscribeOnEvents()
        {
            _connection.On<EchoedEventData>("OnEchoed", (eventData) =>
            {
                OnEchoed?.Invoke(this, eventData);
            });
            _connection.On<AmAliveEventData>("OnAmAlive", (eventData) =>
            {
                OnAmAlive?.Invoke(this, eventData);
            });

            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync().ConfigureAwait(false);

                ConnectionId = _connection.ConnectionId;
            }
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
