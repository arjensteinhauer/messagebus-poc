using MB.Utilities;
using MB.Utilities.AccessTokenProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MB.Access.Tenant.Database
{
    public class TenantContextFactory : ITenantContextFactory<TenantContext>
    {
        private readonly DbContextOptions<TenantContext> _dbContextOptions;
        private readonly IConfiguration _configuration;
        private readonly IAccessTokenProvider _accessTokenProvider;

        public TenantContextFactory(
            DbContextOptions<TenantContext> dbContextOptions,
            IConfiguration configuration,
            IAccessTokenProvider accessTokenProvider)
        {
            _dbContextOptions = dbContextOptions;
            _configuration = configuration;
            _accessTokenProvider = accessTokenProvider;
        }

        public TenantContext Create()
        {
            return new TenantContext(_dbContextOptions, _configuration, _accessTokenProvider);
        }

        public TenantContext Create(string tenantName)
        {
            return new TenantContext(_dbContextOptions, _configuration, _accessTokenProvider, tenantName);
        }
    }
}
