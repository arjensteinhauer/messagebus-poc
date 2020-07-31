using MassTransit;
using MB.Manager.Message1.Interface.V1;
using MB.Utilities.MessageBus;
using System.Threading.Tasks;

namespace MB.Manager.Message1.Proxy.V1
{
    public class Message1ManagerClient : IMessage1Manager
    {
        private readonly IRequestResponseClient<EchoRequest, EchoResponse> _requestResponseClientEcho;
        private readonly IRequestResponseClient<RequestResponseRequest, RequestResponseResponse> _requestResponseClientRequestResponse;
        private readonly IPublishEndpoint _publishEndpoint;

        public Message1ManagerClient(
            IRequestResponseClient<EchoRequest, EchoResponse> requestResponseClientEcho,
            IRequestResponseClient<RequestResponseRequest, RequestResponseResponse> requestResponseClientRequestResponse,
            IPublishEndpoint publishEndpoint)
        {
            _requestResponseClientEcho = requestResponseClientEcho;
            _requestResponseClientRequestResponse = requestResponseClientRequestResponse;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<EchoResponse> Echo(EchoRequest request)
        {
            return await _requestResponseClientEcho.RequestResponseOperation(request);
        }

        public async Task OneWay(OneWayCommand command)
        {
            await _publishEndpoint.Publish<OneWayCommand>(command);
        }

        public async Task<RequestResponseResponse> RequestResponse(RequestResponseRequest request)
        {
            return await _requestResponseClientRequestResponse.RequestResponseOperation(request);
        }
    }
}
