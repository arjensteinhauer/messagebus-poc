using MB.Client.Gateway.Service.Contracts;
using Refit;
using System.Threading.Tasks;

namespace MB.Client.Desktop.App.Proxies
{
    [Headers("TenantName: tenant2")]
    public interface IMessage2Service
    {
        [Post("/message2/echo")]
        Task<string> Echo([Body] EchoRequest request, [Header("Message2-Connection-Id")] string connectionId);
    }
}
