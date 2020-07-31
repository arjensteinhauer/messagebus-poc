using MB.Messaging;
using System;
using System.Threading.Tasks;

namespace MB.Utilities.MessageBus
{
    /// <summary>
    /// A generic class to support the request-response pattern over the message bus.
    /// </summary>
    /// <typeparam name="TRequest">The request message type.</typeparam>
    /// <typeparam name="TResponse">The response message type.</typeparam>
    public interface IRequestResponseClient<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        /// <summary>
        /// The operation to send a request and wait for the response via the message bus.
        /// </summary>
        /// <param name="request">Request message to send.</param>
        /// <returns>The received response message.</returns>
        Task<TResponse> RequestResponseOperation(TRequest request);

        /// <summary>
        /// The operation to send a request and wait for the response via the message bus.
        /// </summary>
        /// <param name="request">The Request message to send.</param>
        /// <param name="faultResponseConfigurations">
        /// An array of possible fault responses with corresponding exceptions.
        /// Each FaultResponseConfiguration registers a function that turns a received fault message into an actual exception.
        /// 
        /// Using the IFault interface, we serialize exceptions on the 'sending side' into a bus message, such that it can be deserialized again on the consuming side.
        /// </param>
        /// <returns>The received response message.</returns>
        Task<TResponse> RequestResponseOperation(TRequest request, params FaultResponseConfiguration<IFault>[] faultResponseConfigurations);
    }

    public class FaultResponseConfiguration<IFault>
    {
        public Type FaultResponseMessageType { get; }

        public Func<IFault, Exception> FaultExceptionFactory { get; }

        public FaultResponseConfiguration(Type faultResponseMessageType, Func<IFault, Exception> faultExceptionFactory)
        {
            if (null == faultExceptionFactory)
            {
                throw new ArgumentException("A FaultResponseConfiguration is meant to translate IFault instances to an exception. Please provide a faultExceptionFactory to the FaultResponseConfiguration.");
            }
            FaultResponseMessageType = faultResponseMessageType;
            FaultExceptionFactory = faultExceptionFactory;
        }
    }
}