using MB.Manager.Message1.Interface.V1;
using Microsoft.AspNetCore.SignalR;

namespace MB.Client.Gateway.Service.Hubs.V1
{
    public class Message1Hub : Hub<IMessage1ManagerEvents>
    {
    }
}