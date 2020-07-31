using MassTransit;
using MB.Client.Gateway.Interface.V1;
using System.Threading.Tasks;

namespace MB.Client.Gateway.Service.EventProxies.V1
{
    public class EventsPublisher : IAmAliveEvent
    {
        private readonly IPublishEndpoint _publishEndPoint;

        public EventsPublisher(IPublishEndpoint publishEndPoint)
        {
            _publishEndPoint = publishEndPoint;
        }

        public async Task OnAmAlive(AmAliveEventData eventData)
        {
            await _publishEndPoint.Publish<AmAliveEventData>(eventData);
        }
    }
}
