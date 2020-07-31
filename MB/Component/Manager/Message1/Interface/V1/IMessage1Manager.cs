using System.Threading.Tasks;

namespace MB.Manager.Message1.Interface.V1
{
    public interface IMessage1Manager
    {
        Task<EchoResponse> Echo(EchoRequest request);

        Task OneWay(OneWayCommand command);

        Task<RequestResponseResponse> RequestResponse(RequestResponseRequest request);
    }
}
