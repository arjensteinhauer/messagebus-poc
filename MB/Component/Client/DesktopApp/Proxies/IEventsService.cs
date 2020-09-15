using MB.Client.Gateway.Service.Contracts;
using Refit;
using System.Threading.Tasks;

namespace MB.Client.Desktop.App.Proxies
{
    [Headers("TenantName: tenant2")]
    public interface IEventsService
    {
        [Post("/events/iAmAlive")]
        Task<string> IAmAlive([Body] IAmAliveRequest request, [Header("Message1-Connection-Id")] string message1ConnectionId, [Header("Message2-Connection-Id")] string message2ConnectionId);
    }
}
