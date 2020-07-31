using MB.Manager.Message2.Interface.V1;
using Microsoft.AspNetCore.SignalR;

namespace MB.Client.Gateway.Service.Hubs.V1
{
    public class Message2Hub : Hub<IMessage2ManagerEvents>
    {
    }
}