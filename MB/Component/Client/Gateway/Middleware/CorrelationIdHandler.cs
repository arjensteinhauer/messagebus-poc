using MB.Messaging;
using MB.Utilities.MessageBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MB.Client.Gateway.Service.Middleware
{
    public class CorrelationIdHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdHandler> _logger;

        public CorrelationIdHandler(RequestDelegate next, ILogger<CorrelationIdHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            /*
             * when provided a correlation-id in the http headers use that one (e.g. for load testing)
             *
             * else
             * asp.net ApplicationInsights Telemetry will start a new trace Activity with rootId.
             *
             */
            string correlationIdString = null;
            if (context.Request.Headers.ContainsKey(CorrelationId.CorrelationIdHttpHeaderKey) && !string.IsNullOrEmpty(context.Request.Headers[CorrelationId.CorrelationIdHttpHeaderKey].ToString()))
            {
                correlationIdString = context.Request.Headers[CorrelationId.CorrelationIdHttpHeaderKey].ToString();
            }
            else if (Activity.Current != null)
            {
                correlationIdString = Activity.Current.RootId;
            }

            if (!string.IsNullOrEmpty(correlationIdString))
            {
                GenericContext<CorrelationId>.Current = new GenericContext<CorrelationId>(new CorrelationId(correlationIdString));
            }

            // apply the correlation ID to the response header for client side tracking and debugging
            context.Response.OnStarting(() =>
            {
                if (!string.IsNullOrEmpty(correlationIdString))
                {
                    context.Response.Headers.Add(CorrelationId.CorrelationIdHttpHeaderKey, correlationIdString);
                }

                return Task.CompletedTask;
            });

            await this._next(context);
        }
    }

    public static class CorrelationIdExtensions
    {
        public static IApplicationBuilder UseCorrelationIdHandler(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<CorrelationIdHandler>();
        }
    }
}
