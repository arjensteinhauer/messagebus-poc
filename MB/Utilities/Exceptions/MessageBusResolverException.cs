using System;
using System.Runtime.Serialization;

namespace MB.Utilities.Exceptions
{
    [Serializable]
    public class MessageBusResolverException : Exception
    {
        public MessageBusResolverException(string messageBusConnectionString, Exception innerException) : base($"Unable to resolve MessageBus. Please check the connection string '{messageBusConnectionString}' and/or the inner exception and try again.", innerException)
        {
        }

        protected MessageBusResolverException(string messageBusConnectionString) : base($"Unable to resolve MessageBus. Please check the connection string '{messageBusConnectionString}' and/or the inner exception and try again.")
        {
        }

        protected MessageBusResolverException()
        {
        }

        protected MessageBusResolverException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
