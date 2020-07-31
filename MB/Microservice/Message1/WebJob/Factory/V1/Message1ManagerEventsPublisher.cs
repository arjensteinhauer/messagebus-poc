using MassTransit;
using MB.Manager.Message1.Interface.V1;
using System.Threading.Tasks;

namespace MB.Microservice.Message1.WebJob.Factory.V1
{
    public class Message1ManagerEventsPublisher : IMessage1ManagerEvents
    {
        private readonly IPublishEndpoint _publishEndPoint;

        public Message1ManagerEventsPublisher(IPublishEndpoint publishEndPoint)
        {
            _publishEndPoint = publishEndPoint;
        }

        public async Task OnEchoed(EchoedEventData eventData)
        {
            await _publishEndPoint.Publish<EchoedEventData>(eventData);
        }

        public async Task OnProcessedOneWayCommand(ProcessedOneWayCommandEventData eventData)
        {
            await _publishEndPoint.Publish<ProcessedOneWayCommandEventData>(eventData);
        }

        public async Task OnAmAlive(AmAliveEventData eventData)
        {
            await _publishEndPoint.Publish<AmAliveEventData>(eventData);
        }
    }
}