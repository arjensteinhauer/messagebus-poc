using MB.Client.Gateway.Interface.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MB.Client.Gateway.Service.Controllers.V1
{
    [ApiController]
    [Route("api/v1")]
    public class EventsController : EventsControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IAmAliveEvent _amAliveEventPublisher;

        public EventsController(IAmAliveEvent amAliveEventPublisher, ILogger<EventsController> logger)
        {
            _amAliveEventPublisher = amAliveEventPublisher;
            _logger = logger;
        }

        public override async Task<string> IAmAlive([BindRequired, FromBody] IAmAliveRequest body)
        {
            await _amAliveEventPublisher.OnAmAlive(new AmAliveEventData { Message = body?.Input });
            return $"I am alive event message '{body?.Input}' delivered to the Message Bus...";
        }
    }
}
