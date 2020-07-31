using System.Threading.Tasks;

namespace MB.Manager.Message2.Interface.V1
{
    public interface IMessage2Manager
    {
        Task<EchoResponse> Echo(EchoRequest request);
    }
}
