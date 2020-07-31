using System.Threading.Tasks;

namespace MB.Utilities.AccessTokenProvider
{
    public interface IAccessTokenProvider
    {
        public Task<string> GetSqlDbAccessToken();
    }
}
