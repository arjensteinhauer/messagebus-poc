using MB.Manager.Message2.Interface.V1;
using MB.Utilities.MessageBus;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MB.Client.Gateway.Service.Hubs.V1
{
    public class Message2EventsHandler : IEchoedEvent, IAmAliveEvent
    {
        private readonly ILogger _logger;
        private readonly IHubContext<Message2Hub, IMessage2ManagerEvents> _hubContext;

        public Message2EventsHandler(IHubContext<Message2Hub, IMessage2ManagerEvents> hubContext, ILoggerFactory loggerFactory)
        {
            _hubContext = hubContext;
            _logger = loggerFactory.CreateLogger<Message2EventsHandler>();
        }

        public async Task OnEchoed(EchoedEventData eventData)
        {
            _logger.LogInformation($"{eventData.Result}\t\t -> \t {GetType().Namespace}.{GetType().Name}.{nameof(OnEchoed)}");

            var connectionId = GenericContext<Message2ConnectionId>.Current?.Value;
            if (connectionId != null && connectionId.Value != null)
            {
                await _hubContext.Clients.Client(connectionId.Value).OnEchoed(eventData).ConfigureAwait(false);
            }
        }

        public async Task OnAmAlive(AmAliveEventData eventData)
        {
            _logger.LogInformation($"{eventData.Message}\t\t -> \t {GetType().Namespace}.{GetType().Name}.{nameof(OnAmAlive)}");

            var connectionId = GenericContext<Message2ConnectionId>.Current?.Value;
            if (connectionId != null && connectionId.Value != null)
            {
                await _hubContext.Clients.Client(connectionId.Value).OnAmAlive(eventData).ConfigureAwait(false);
            }
        }
    }
}