using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MB.Client.Gateway.Service.Hubs.V1;
using MB.Utilities.MessageBus;
using System.Threading.Tasks;

namespace MB.Client.Gateway.Service.Middleware
{
    public class SignalRConnectionIdHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SignalRConnectionIdHandler> _logger;

        public SignalRConnectionIdHandler(RequestDelegate next, ILogger<SignalRConnectionIdHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(Message1ConnectionId.HeaderKey))
            {
                var connectionIdHeaderValue = context.Request.Headers[Message1ConnectionId.HeaderKey].ToString();
                if (!string.IsNullOrWhiteSpace(connectionIdHeaderValue))
                {
                    _logger.LogDebug($"Added {Message1ConnectionId.ItemKey} to the OperationContext");
                    GenericContext<Message1ConnectionId>.Current = new GenericContext<Message1ConnectionId>(new Message1ConnectionId(connectionIdHeaderValue));
                }
            }

            if (context.Request.Headers.ContainsKey(Message2ConnectionId.HeaderKey))
            {
                var connectionIdHeaderValue = context.Request.Headers[Message2ConnectionId.HeaderKey].ToString();
                if (!string.IsNullOrWhiteSpace(connectionIdHeaderValue))
                {
                    _logger.LogDebug($"Added {Message2ConnectionId.ItemKey} to the OperationContext");
                    GenericContext<Message2ConnectionId>.Current = new GenericContext<Message2ConnectionId>(new Message2ConnectionId(connectionIdHeaderValue));
                }
            }
            
            await _next(context);
        }
    }

    public static class SignalRConnectionIdHandlerExtensions
    {
        public static IApplicationBuilder UseSignalRConnectionIdHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SignalRConnectionIdHandler>();
        }
    }
}