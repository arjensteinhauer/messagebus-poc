using GreenPipes;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MB.Utilities.MessageBus
{
    /// <summary>
    /// A generic class to support the request-response pattern over the message bus.
    /// </summary>
    /// <typeparam name="TRequest">The request message type.</typeparam>
    /// <typeparam name="TResponse">The response message type.</typeparam>
    public class RequestResponseClient<TRequest, TResponse> : IRequestResponseClient<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        /// <summary>
        /// MassTransit RequestClient is used for the request-response pattern support.
        /// </summary>
        private readonly IRequestClient<TRequest> _requestClient;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="requestClient">The request client.</param>
        public RequestResponseClient(IRequestClient<TRequest> requestClient)
        {
            _requestClient = requestClient;
        }

        /// <summary>
        /// The operation to send a request and wait for the response via the message bus.
        /// </summary>
        /// <param name="request">Request message to send.</param>
        /// <returns>The received response message.</returns>
        public async Task<TResponse> RequestResponseOperation(TRequest request)
        {
            using (var handle = _requestClient.Create(request))
            {
                handle.UseExecute(sendContext =>
                {
                    sendContext.Headers.AddToMassTransitMessage(OperationContext.Current?.OutgoingHeaders);
                });

                var response = await handle.GetResponse<TResponse>();
                return response.Message;
            }
        }

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
        public async Task<TResponse> RequestResponseOperation(TRequest request, params FaultResponseConfiguration<Messaging.IFault>[] faultResponseConfigurations)
        {
            using (var handle = _requestClient.Create(request))
            {
                // add the generic operation context to the request message headers
                handle.UseExecute(sendContext =>
                {
                    sendContext.Headers.AddToMassTransitMessage(OperationContext.Current?.OutgoingHeaders);
                });
                var responseTasks = new List<Task>();

                // Create tasks that wait for an expected fault response message.
                // The Consumer of the request object, might respond with a response object of one or more types
                // in our case, the consumer might send back the expected TResponse,
                // but also a  message of type IFault (Like PartyNotFoundFault)
                // see for example MembershipManagerConsumer.cs
                //
                // MassTransit allows you to await multiple types by calling GetResponse for each expected response type.
                // https://masstransit-project.com/usage/requests.html#request-client
                // "To add additional response types, use the tuple syntax, or just add multiple GetResponse methods, passing false for the readyToSend parameter."
                foreach (var faultResponseConfiguration in faultResponseConfigurations)
                {
                    // Here we want to call a generic method, however the Type argument is dynamic and not known at compile time.
                    // We use some reflection to call the generic GetResponse<T> message with the type specified in the FaultResponseConfiguration.
                    var faultResponseTask = ((TypeInfo)handle.GetType())
                        .GetDeclaredMethod("MassTransit.RequestHandle.GetResponse")
                        .MakeGenericMethod(faultResponseConfiguration.FaultResponseMessageType)
                        .Invoke(handle, new object[] { false }) as Task; // according to MassTransit documentation: Should be false when expecting mulitple response types on the same request

                    responseTasks.Add(faultResponseTask);
                }

                // Create a task to wait for the expected response message (normal operation)
                var expectedResponseTask = handle.GetResponse<TResponse>();
                responseTasks.Add(expectedResponseTask);

                // Wait for all response tasks to complete
                var completedResponseTask = await Task.WhenAny(responseTasks);
                switch (completedResponseTask.Status)
                {
                    // The Returned Task will be the first task that is RanToCompletion, Faulted, or Canceled.
                    case TaskStatus.Canceled:
                        /*
                         * The Canceled TaskStatus is a case that comes from the .net Task APi.
                         * We don't expect the user to cancel a particular message bus and hence don't support it here.
                         * In theory any underlying framework might trigger the cancellation as well.
                         * That is still considered a situation we don't support, hence we throw a custom exception here.
                         */
                        var responseMessageType = completedResponseTask.GetResponseMessageType();
                        throw new ResponseTaskCancelledException($"A MassTransit GetResponse task was cancelled. Expecting response message type {responseMessageType.Name} for request message type {typeof(TRequest).Name}");

                    case TaskStatus.Faulted:
                        throw completedResponseTask.Exception;

                    case TaskStatus.RanToCompletion:
                        // on normal operation, return the received response message
                        if (completedResponseTask == expectedResponseTask)
                        {
                            return expectedResponseTask.Result.Message;
                        }

                        // on fault, get the function that translates the fault response message to an exception
                        var faultMessage = completedResponseTask.GetResponseMessage<Messaging.IFault>();
                        var faultResponseMessageType = completedResponseTask.GetResponseMessageType();
                        var faultExceptionFactory = faultResponseConfigurations
                            .Where(c => c.FaultResponseMessageType.Equals(faultResponseMessageType))
                            .Select(c => c.FaultExceptionFactory)
                            .SingleOrDefault();

                        var faultException = faultExceptionFactory(faultMessage);

                        // throw the corresponding exception
                        throw faultException;

                    default:
                        throw new InvalidOperationException($"The completed MassTransit response task, has a task status other then {nameof(TaskStatus.Canceled)}, {nameof(TaskStatus.Faulted)}, {nameof(TaskStatus.RanToCompletion)}. The task status '{completedResponseTask.Status}' is unexpected.");
                }
            }
        }
    }
}