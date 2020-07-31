using MassTransit;
using MB.Manager.Message2.Interface.V1;
using System.Threading.Tasks;

namespace MB.Microservice.Message2.WebJob.Factory.V1
{
    public class Message2ManagerEventsPublisher : IMessage2ManagerEvents
    {
        private readonly IPublishEndpoint _publishEndPoint;

        public Message2ManagerEventsPublisher(IPublishEndpoint publishEndPoint)
        {
            _publishEndPoint = publishEndPoint;
        }

        public async Task OnEchoed(EchoedEventData eventData)
        {
            await _publishEndPoint.Publish<EchoedEventData>(eventData);
        }

        public async Task OnAmAlive(AmAliveEventData eventData)
        {
            await _publishEndPoint.Publish<AmAliveEventData>(eventData);
        }
    }
}