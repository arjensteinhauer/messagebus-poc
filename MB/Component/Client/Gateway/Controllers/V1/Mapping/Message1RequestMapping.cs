using ApiEchoRequest = MB.Client.Gateway.Service.Controllers.EchoRequest;
using ApiOneWayRequest = MB.Client.Gateway.Service.Controllers.OneWayRequest;
using ApiRequestResponseRequest = MB.Client.Gateway.Service.Controllers.RequestResponseRequest;
using ManagerEchoRequest = MB.Manager.Message1.Interface.V1.EchoRequest;
using ManagerOneWayCommand = MB.Manager.Message1.Interface.V1.OneWayCommand;
using ManagerRequestResponseRequest = MB.Manager.Message1.Interface.V1.RequestResponseRequest;

namespace MB.Client.Gateway.Service.Controllers.V1.Mapping
{
    public static class Message1RequestMapping
    {
        public static ManagerEchoRequest Message1Map(this ApiEchoRequest api)
        {
            if (api == null)
            {
                return null;
            }
            var manager = new ManagerEchoRequest();
            manager.Input = api.Input;
            return manager;
        }

        public static ManagerRequestResponseRequest Message1Map(this ApiRequestResponseRequest api)
        {
            if (api == null)
            {
                return null;
            }
            var manager = new ManagerRequestResponseRequest();
            manager.Input = api.Input;
            return manager;
        }

        public static ManagerOneWayCommand Message1Map(this ApiOneWayRequest api)
        {
            if (api == null)
            {
                return null;
            }
            var manager = new ManagerOneWayCommand();
            manager.Input = api.Input;
            return manager;
        }
    }
}
