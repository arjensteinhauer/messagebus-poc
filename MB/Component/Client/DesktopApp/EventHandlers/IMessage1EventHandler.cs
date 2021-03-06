﻿using MB.Manager.Message1.Interface.V1;
using System;
using System.Threading.Tasks;

namespace MB.Client.Desktop.App.EventHandlers
{
    public interface IMessage1EventHandler
    {
        /// <summary>
        /// Event handler for handling OnEchoed events.
        /// </summary>
        event EventHandler<EchoedEventData> OnEchoed;

        /// <summary>
        /// Event handler for handling OnProcessedOneWayCommand events.
        /// </summary>
        event EventHandler<ProcessedOneWayCommandEventData> OnProcessedOneWayCommand;

        /// <summary>
        /// Event handler for handling OnAmAlive events.
        /// </summary>
        event EventHandler<AmAliveEventData> OnAmAlive;

        /// <summary>
        /// Event handler for handling OnPublishSomething events.
        /// </summary>
        event EventHandler<PublishSomethingEventData> OnPublishSomething;

        /// <summary>
        /// SignalR connection ID.
        /// </summary>
        string ConnectionId { get; }

        /// <summary>
        /// Subscribe on events.
        /// </summary>
        Task SubscribeOnEvents();

        /// <summary>
        /// Subscribe on events for a specific name.
        /// </summary>
        /// <param name="subscriptionName">Name of the </param>
        Task SubscribeOnEventsFor(string subscriptionName);

        /// <summary>
        /// Unsubscribe on events for a specific name.
        /// </summary>
        /// <param name="subscriptionName">Name of the </param>
        Task UnsubscribeOnEventsFor(string subscriptionName);

        /// <summary>
        /// Disconnect the signalR connection.
        /// </summary>
        /// <returns>Async task.</returns>
        Task Disconnect();
    }
}
