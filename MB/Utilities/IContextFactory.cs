using Microsoft.EntityFrameworkCore;

namespace MB.Utilities
{
    public interface IContextFactory<T> where T : DbContext
    {
        T Create();
    }
}
