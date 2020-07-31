using GreenPipes;
using MassTransit;
using MassTransit.Context;
using MB.Utilities.Exceptions;
using MB.Utilities.Extensions;
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

                        // set operation context sending/publishing messages
                        configure.ConfigurePublish(pipeConfig => pipeConfig.UseExecute(publishContext => publishContext.Headers.AddToMassTransitMessage(OperationContext.Current?.OutgoingHeaders)));
                        configure.ConfigureSend(pipeConfig => pipeConfig.UseExecute(sendContext => sendContext.Headers.AddToMassTransitMessage(OperationContext.Current?.OutgoingHeaders)));

                        // get operation context consuming messages
                        configure.UseExecute(consumeContext => _ = new OperationContext(consumeContext.Headers.FromMassTransitMessage()));
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

                        // set operation context sending/publishing messages
                        configure.ConfigurePublish(pipeConfig => pipeConfig.UseExecute(publishContext => publishContext.Headers.AddToMassTransitMessage(OperationContext.Current?.OutgoingHeaders)));
                        configure.ConfigureSend(pipeConfig => pipeConfig.UseExecute(sendContext => sendContext.Headers.AddToMassTransitMessage(OperationContext.Current?.OutgoingHeaders)));

                        // get operation context consuming messages
                        configure.UseExecute(consumeContext => _ = new OperationContext(consumeContext.Headers.FromMassTransitMessage()));
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
