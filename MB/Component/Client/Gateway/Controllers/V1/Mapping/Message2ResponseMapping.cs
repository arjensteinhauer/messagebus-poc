using ManagerEchoResponse = MB.Manager.Message2.Interface.V1.EchoResponse;

namespace MB.Client.Gateway.Service.Controllers.V1.Mapping
{
    public static class Message2ResponseMapping
    {
        public static string Map(this ManagerEchoResponse manager)
        {
            if (manager == null)
            {
                return null;
            }
            return manager.Result;
        }
    }
}
