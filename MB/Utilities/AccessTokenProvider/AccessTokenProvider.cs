using Microsoft.Azure.Services.AppAuthentication;
using System.Threading.Tasks;

namespace MB.Utilities.AccessTokenProvider
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        public async Task<string> GetSqlDbAccessToken()
        {
            var provider = new AzureServiceTokenProvider();
            return await provider.GetAccessTokenAsync("https://database.windows.net/").ConfigureAwait(false);
        }
    }
}
