using System;
using System.Threading.Tasks;

namespace MB.Utilities.MessageBus
{
    public interface IEventSubscriber : IDisposable
    {
        public Task Subscribe<TEvent>(string handlerName, Type consumerType, Func<Type, object> consumerFactory) where TEvent : class;

        public Task Unsubscribe<TEvent>(string handlerName);
    }
}
