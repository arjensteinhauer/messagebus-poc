using ManagerEchoRequest = MB.Manager.Message2.Interface.V1.EchoRequest;
using ApiEchoRequest = MB.Client.Gateway.Service.Controllers.EchoRequest;

namespace MB.Client.Gateway.Service.Controllers.V1.Mapping
{
    public static class Message2RequestMapping
    {
        public static ManagerEchoRequest Message2Map(this ApiEchoRequest api)
        {
            if (api == null)
            {
                return null;
            }
            var manager = new ManagerEchoRequest();
            manager.Input = api.Input;
            return manager;
        }
    }
}
