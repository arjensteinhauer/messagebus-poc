using Microsoft.EntityFrameworkCore;

namespace MB.Utilities
{
    public interface ITenantContextFactory<T> : IContextFactory<T> where T : DbContext
    {
        T Create(string tenantName);
    }
}
