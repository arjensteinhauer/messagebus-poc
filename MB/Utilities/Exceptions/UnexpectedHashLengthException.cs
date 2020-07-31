using System;
using System.Runtime.Serialization;

namespace MB.Utilities.Exceptions
{
    [Serializable]
    public class UnexpectedHashLengthException : Exception
    {
        internal UnexpectedHashLengthException(string message) : base(message)
        {
        }

        protected UnexpectedHashLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnexpectedHashLengthException() : base()
        {
        }

        protected UnexpectedHashLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
