using System;
using System.Runtime.Serialization;

namespace MB.Utilities.MessageBus
{
    [Serializable]
    internal class ResponseTaskCancelledException : Exception
    {
        public ResponseTaskCancelledException()
        {
        }

        public ResponseTaskCancelledException(string message) : base(message)
        {
        }

        public ResponseTaskCancelledException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ResponseTaskCancelledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}