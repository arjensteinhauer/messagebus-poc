using MB.Manager.Message1.Interface.V1;
using MB.Utilities.MessageBus;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MB.Client.Gateway.Service.Hubs.V1
{
    public class Message1EventsHandler : IEchoedEvent, IProcessedOneWayCommandEvent, IAmAliveEvent, IPublishSomethingEvent
    {
        private readonly ILogger _logger;
        private readonly IHubContext<Message1Hub, IMessage1ManagerEvents> _hubContext;

        public Message1EventsHandler(IHubContext<Message1Hub, IMessage1ManagerEvents> hubContext, ILoggerFactory loggerFactory)
        {
            _hubContext = hubContext;
            _logger = loggerFactory.CreateLogger<Message1EventsHandler>();
        }

        public async Task OnEchoed(EchoedEventData eventData)
        {
            _logger.LogInformation($"{eventData.Result}\t\t -> \t {GetType().Namespace}.{GetType().Name}.{nameof(OnEchoed)}");

            var connectionId = GenericContext<Message1ConnectionId>.Current?.Value;
            if (connectionId != null && connectionId.Value != null)
            {
                await _hubContext.Clients.Client(connectionId.Value).OnEchoed(eventData).ConfigureAwait(false);
            }
        }

        public async Task OnProcessedOneWayCommand(ProcessedOneWayCommandEventData eventData)
        {
            _logger.LogInformation($"{eventData.Result}\t\t -> \t {GetType().Namespace}.{GetType().Name}.{nameof(OnProcessedOneWayCommand)}");

            var connectionId = GenericContext<Message1ConnectionId>.Current?.Value;
            if (connectionId != null && connectionId.Value != null)
            {
                await _hubContext.Clients.Client(connectionId.Value).OnProcessedOneWayCommand(eventData).ConfigureAwait(false);
            }
        }

        public async Task OnAmAlive(AmAliveEventData eventData)
        {
            _logger.LogInformation($"{eventData.Message}\t\t -> \t {GetType().Namespace}.{GetType().Name}.{nameof(OnAmAlive)}");

            var connectionId = GenericContext<Message1ConnectionId>.Current?.Value;
            if (connectionId != null && connectionId.Value != null)
            {
                await _hubContext.Clients.Client(connectionId.Value).OnAmAlive(eventData).ConfigureAwait(false);
            }
        }

        public async Task OnPublishSomething(PublishSomethingEventData eventData)
        {
            _logger.LogInformation($"{GetType().Namespace}.{GetType().Name}.{nameof(OnPublishSomething)} for '{eventData.Name}':\n\t--> {eventData.Message}");

            await _hubContext.Clients.Group(eventData.Name).OnPublishSomething(eventData).ConfigureAwait(false);
        }
    }
}