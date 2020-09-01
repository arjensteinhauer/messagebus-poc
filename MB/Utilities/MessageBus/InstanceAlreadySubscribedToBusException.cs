using System;
using System.Runtime.Serialization;

namespace MB.Utilities.MessageBus
{
    [Serializable]
    public class InstanceAlreadySubscribedToBusException : Exception
    {
        public InstanceAlreadySubscribedToBusException(string name) : base($"Name: {name}")
        {
        }

        protected InstanceAlreadySubscribedToBusException(string name, Exception innerException) : base($"Name: {name}", innerException)
        {
        }

        protected InstanceAlreadySubscribedToBusException()
        {
        }

        protected InstanceAlreadySubscribedToBusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}