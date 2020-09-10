using System.Threading.Tasks;

namespace MB.Access.Tenant.Interface.V1
{
    public interface ITenantAccess
    {
        Task<string> Echo(string input);

        Task<StoreMessageResponse> StoreMessage(StoreMessageRequest request);
    }
}
