using MB.Manager.Message2.Interface.V1;
using System;
using System.Threading.Tasks;

namespace MB.Client.Desktop.App.EventHandlers
{
    public interface IMessage2EventHandler
    {
        /// <summary>
        /// Event handler for handling OnEchoed events.
        /// </summary>
        event EventHandler<EchoedEventData> OnEchoed;

        /// <summary>
        /// Event handler for handling OnAmAlive events.
        /// </summary>
        event EventHandler<AmAliveEventData> OnAmAlive;

        /// <summary>
        /// SignalR connection ID.
        /// </summary>
        string ConnectionId { get; }

        /// <summary>
        /// Subscribe on events.
        /// </summary>
        Task SubscribeOnEvents();

        /// <summary>
        /// Disconnect the signalR connection.
        /// </summary>
        /// <returns>Async task.</returns>
        Task Disconnect();
    }
}
