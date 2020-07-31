using MassTransit;
using MB.Manager.Message2.Interface.V1;
using System.Threading.Tasks;

namespace MB.Manager.Message2.Proxy.V1
{
    public class EchoedEventConsumer : IConsumer<EchoedEventData>
    {
        private readonly IEchoedEvent _eventHandler;

        public EchoedEventConsumer(IEchoedEvent eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task Consume(ConsumeContext<EchoedEventData> context)
        {
            await _eventHandler.OnEchoed(context.Message);
        }
    }
}
