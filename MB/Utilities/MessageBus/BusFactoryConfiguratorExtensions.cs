using GreenPipes;
using MassTransit;

namespace MB.Utilities.MessageBus
{
    public static class BusFactoryConfiguratorExtensions
    {
        public static IBusFactoryConfigurator<T> UseMessageContext<T>(this IBusFactoryConfigurator<T> configure) where T: IReceiveEndpointConfigurator
        {
            // set operation context sending/publishing messages
            configure.ConfigurePublish(pipeConfig => pipeConfig.UseExecute(publishContext => publishContext.Headers.AddToMassTransitMessage(OperationContext.Current?.OutgoingHeaders)));
            configure.ConfigureSend(pipeConfig => pipeConfig.UseExecute(sendContext => sendContext.Headers.AddToMassTransitMessage(OperationContext.Current?.OutgoingHeaders)));

            // get operation context consuming messages
            configure.UseExecute(consumeContext => _ = new OperationContext(consumeContext.Headers.FromMassTransitMessage()));

            // done
            return configure;
        }

        public static IBusFactoryConfigurator UseMessageContext(this IBusFactoryConfigurator configure)
        {
            // set operation context sending/publishing messages
            configure.ConfigurePublish(pipeConfig => pipeConfig.UseExecute(publishContext => publishContext.Headers.AddToMassTransitMessage(OperationContext.Current?.OutgoingHeaders)));
            configure.ConfigureSend(pipeConfig => pipeConfig.UseExecute(sendContext => sendContext.Headers.AddToMassTransitMessage(OperationContext.Current?.OutgoingHeaders)));

            // get operation context consuming messages
            configure.UseExecute(consumeContext => _ = new OperationContext(consumeContext.Headers.FromMassTransitMessage()));

            // done
            return configure;
        }
    }
}
