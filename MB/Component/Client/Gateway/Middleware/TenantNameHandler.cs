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
    public class TenantNameHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdHandler> _logger;

        public TenantNameHandler(RequestDelegate next, ILogger<CorrelationIdHandler> logger)
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
            string tenantNameString = null;
            if (context.Request.Headers.ContainsKey(TenantName.TenantNameHttpHeaderKey) && !string.IsNullOrEmpty(context.Request.Headers[TenantName.TenantNameHttpHeaderKey].ToString()))
            {
                tenantNameString = context.Request.Headers[TenantName.TenantNameHttpHeaderKey].ToString();
            }

            if (!string.IsNullOrEmpty(tenantNameString))
            {
                GenericContext<TenantName>.Current = new GenericContext<TenantName>(new TenantName(tenantNameString));
            }

            await this._next(context);
        }
    }

    public static class TenantNameExtensions
    {
        public static IApplicationBuilder UseTenantNameHandler(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<TenantNameHandler>();
        }
    }
}
