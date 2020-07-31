using System.IO;
using System.Threading.Tasks;

namespace MB.Utilities
{
    public class NullFileClient : IFileClient
    {
        public async Task DeleteFile(string filePath)
        {
            await Task.CompletedTask;
        }

        public async Task<bool> FileExists(string filePath)
        {
            return await Task.FromResult(false);
        }

        public async Task<Stream> GetFile(string filePath)
        {
            return await Task.FromResult((Stream)null);
        }

        public async Task<string> GetFileUrl(string filePath)
        {
            return await Task.FromResult((string)null);
        }

        public async Task SaveFile(string filePath, Stream contentStream)
        {
            await Task.CompletedTask;
        }
    }
}
