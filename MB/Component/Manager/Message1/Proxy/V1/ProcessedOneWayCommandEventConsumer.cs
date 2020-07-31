using MassTransit;
using MB.Manager.Message1.Interface.V1;
using System.Threading.Tasks;

namespace MB.Manager.Message1.Proxy.V1
{
    public class ProcessedOneWayCommandEventConsumer : IConsumer<ProcessedOneWayCommandEventData>
    {
        private readonly IProcessedOneWayCommandEvent _eventHandler;

        public ProcessedOneWayCommandEventConsumer(IProcessedOneWayCommandEvent eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public async Task Consume(ConsumeContext<ProcessedOneWayCommandEventData> context)
        {
            await _eventHandler.OnProcessedOneWayCommand(context.Message);
        }
    }
}
