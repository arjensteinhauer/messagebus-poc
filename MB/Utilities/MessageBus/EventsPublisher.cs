using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MB.Utilities.MessageBus
{
    public class EventsPublisher<TEventsContract>
        where TEventsContract : class
    {
        private readonly IPublishEndpoint _publishEndPoint;

        public EventsPublisher(IPublishEndpoint publishEndPoint, TEventsContract eventsContract)
        {
            _publishEndPoint = publishEndPoint;
        }

        public async Task Publish(TEventsContract eventData)
        {
            await _publishEndPoint.Publish(eventData);
        }
    }
}
