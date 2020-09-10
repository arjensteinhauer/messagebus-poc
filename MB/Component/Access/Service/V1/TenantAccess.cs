using MB.Access.Tenant.Database;
using MB.Access.Tenant.Interface.V1;
using MB.Messaging;
using MB.Utilities;
using MB.Utilities.MessageBus;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MB.Access.Tenant.Service.V1
{
    public class TenantAccess : ITenantAccess
    {
        private readonly ITenantContextFactory<TenantContext> _contextFactory;
        private readonly ILogger _logger;

        public TenantAccess(ITenantContextFactory<TenantContext> contextFactory, ILoggerFactory loggerFactory)
        {
            _contextFactory = contextFactory;
            _logger = loggerFactory.CreateLogger<TenantAccess>();
        }

        public async Task<string> Echo(string input)
        {
            var result = $"{input} -> {GetType().Namespace}.{GetType().Name}.{nameof(Echo)} ";
            try
            {
                using (var context = _contextFactory.Create())
                {
                    var canConnect = await context.Database.CanConnectAsync();
                    result += $"-> Database {context.GetType().Name}, can connect: {canConnect}";
                }
            }
            catch (Exception ex)
            {
                result += $"-> Database, error: {ex.Message}";
            }

            _logger.LogInformation(result);
            return result;
        }


        public async Task<StoreMessageResponse> StoreMessage(StoreMessageRequest request)
        {
            var tenantName = GenericContext<TenantName>.Current?.Value?.Value;

            using (var context = _contextFactory.Create(tenantName))
            {
                var message = new Database.Models.Message
                {
                    Subject = request.MessageDetails.Subject,
                    Body = request.MessageDetails.Body,
                    PublishedOnUTC = DateTimeOffset.UtcNow
                };

                await context.Messages.AddAsync(message);
                await context.SaveChangesAsync();

                return new StoreMessageResponse { MessageId = message.MessageId };
            }
        }
    }
}