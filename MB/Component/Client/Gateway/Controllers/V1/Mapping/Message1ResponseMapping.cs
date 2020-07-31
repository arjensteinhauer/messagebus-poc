using MB.Manager.Message1.Interface.V1;

namespace MB.Client.Gateway.Service.Controllers.V1.Mapping
{
    public static class Message1ResponseMapping
    {
        public static string Map(this EchoResponse response)
        {
            if (response == null)
            {
                return null;
            }
            return response.Result;
        }

        public static string Map(this RequestResponseResponse response)
        {
            if (response == null)
            {
                return null;
            }
            return response.Result;
        }
    }
}
