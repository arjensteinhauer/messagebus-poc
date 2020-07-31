using MB.Client.Gateway.Service.Contracts;
using Refit;
using System.Threading.Tasks;

namespace MB.Client.Desktop.App.Proxies
{
    public interface IMessage1Service
    {
        [Post("/message1/echo")]
        Task<string> Echo([Body] EchoRequest request, [Header("Message1-Connection-Id")] string connectionId);

        [Post("/message1/oneWay")]
        Task<string> OneWay([Body] OneWayRequest request, [Header("Message1-Connection-Id")] string connectionId);

        [Post("/message1/requestResponse")]
        Task<string> RequestResponse([Body] RequestResponseRequest request, [Header("Message1-Connection-Id")] string connectionId);
    }
}
