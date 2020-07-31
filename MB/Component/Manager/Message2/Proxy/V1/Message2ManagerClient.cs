using MB.Manager.Message2.Interface.V1;
using MB.Utilities.MessageBus;
using System.Threading.Tasks;

namespace MB.Manager.Message2.Proxy.V1
{
    public class Message2ManagerClient : IMessage2Manager
    {
        private readonly IRequestResponseClient<EchoRequest, EchoResponse> _requestResponseClientEcho;

        public Message2ManagerClient(IRequestResponseClient<EchoRequest, EchoResponse> requestResponseClientEcho)
        {
            _requestResponseClientEcho = requestResponseClientEcho;
        }

        public async Task<EchoResponse> Echo(EchoRequest request)
        {
            return await _requestResponseClientEcho.RequestResponseOperation(request);
        }
    }
}
