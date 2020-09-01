using MassTransit;
using System;
using System.Threading.Tasks;

namespace MB.Utilities.MessageBus
{
    public interface IMessageBusControl : IBusControl, IDisposable
    {
        public Task DisconnectReceiveEndpoint(string queueName);
    }
}
