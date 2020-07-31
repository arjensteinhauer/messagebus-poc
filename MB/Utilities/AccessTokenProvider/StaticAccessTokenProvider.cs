using System.Threading.Tasks;

namespace MB.Utilities.AccessTokenProvider
{
    public class StaticAccessTokenProvider : IAccessTokenProvider
    {
        private readonly string _sqlAccessToken;

        public StaticAccessTokenProvider(string sqlAccessToken)
        {
            _sqlAccessToken = sqlAccessToken;
        }

        public async Task<string> GetSqlDbAccessToken()
        {
            return await Task.FromResult(_sqlAccessToken);
        }
    }
}
