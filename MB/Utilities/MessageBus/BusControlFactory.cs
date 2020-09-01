using MassTransit;
using MassTransit.Context;
using MB.Utilities.Exceptions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace MB.Utilities.MessageBus
{
    public static class BusControlFactory
    {
        public static IBusControl Create(IBusRegistrationContext context)
        {
            var configuration = context.GetService<IConfiguration>();
            var loggerFactory = context.GetService<ILoggerFactory>();

            var messageBusConnectionString = configuration.GetConnectionString("MessageBus");

            try
            {
                if (messageBusConnectionString.StartsWith("rabbitmq://"))
                {
                    var busControl = Bus.Factory.CreateUsingRabbitMq(configure =>
                    {
                        LogContext.ConfigureCurrentLogContext(loggerFactory);
                        configure.Host(new Uri(messageBusConnectionString), _ => { });
                        configure.ConfigureEndpoints(context);

                        // use te the generic message context (operation context) in sending and receiving
                        configure.UseMessageContext();
                    });

                    return busControl;
                }
                else
                {
                    var busControl = Bus.Factory.CreateUsingAzureServiceBus(configure =>
                    {
                        if (messageBusConnectionString.StartsWith("sb://"))
                        {
                            messageBusConnectionString = $"Endpoint={messageBusConnectionString}";
                        }

                        var messageBusConnectionStringBuilder = new ServiceBusConnectionStringBuilder(messageBusConnectionString);

                        LogContext.ConfigureCurrentLogContext(loggerFactory);
                        configure.Host(new Uri(messageBusConnectionStringBuilder.Endpoint), hostConfig =>
                        {
                            hostConfig.TokenProvider = string.IsNullOrEmpty(messageBusConnectionStringBuilder.SasKeyName) || string.IsNullOrEmpty(messageBusConnectionStringBuilder.SasKey)
                                ? TokenProvider.CreateManagedIdentityTokenProvider()
                                : TokenProvider.CreateSharedAccessSignatureTokenProvider(messageBusConnectionStringBuilder.SasKeyName, messageBusConnectionStringBuilder.SasKey);
                        });

                        configure.SetNamespaceSeparatorTo("_");
                        configure.ConfigureEndpoints(context);

                        // use te the generic message context (operation context) in sending and receiving
                        configure.UseMessageContext();
                    });

                    return busControl;
                }
            }
            catch (Exception ex)
            {
                throw new MessageBusResolverException(messageBusConnectionString, ex);
            }
        }
    }
}
