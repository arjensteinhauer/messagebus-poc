using MassTransit;
using MB.Manager.Message1.Interface.V1;
using System.Threading.Tasks;

namespace MB.Manager.Message1.Proxy.V1
{
    public class PublishSomethingEventConsumer : IConsumer<PublishSomethingEventData>
    {
        private readonly IPublishSomethingEvent _eventHandler;

        public PublishSomethingEventConsumer(IPublishSomethingEvent eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task Consume(ConsumeContext<PublishSomethingEventData> context)
        {
            await _eventHandler.OnPublishSomething(context.Message);
        }
    }
}
