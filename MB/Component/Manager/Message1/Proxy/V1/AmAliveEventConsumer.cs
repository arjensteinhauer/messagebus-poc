using MassTransit;
using MB.Manager.Message1.Interface.V1;
using System.Threading.Tasks;

namespace MB.Manager.Message1.Proxy.V1
{
    public class AmAliveEventConsumer : IConsumer<AmAliveEventData>
    {
        private readonly IAmAliveEvent _eventHandler;

        public AmAliveEventConsumer(IAmAliveEvent eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task Consume(ConsumeContext<AmAliveEventData> context)
        {
            await _eventHandler.OnAmAlive(context.Message);
        }
    }
}
