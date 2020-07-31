using System.IO;
using System.Threading.Tasks;

namespace MB.Utilities
{
    public interface IFileClient
    {
        Task DeleteFile(string filePath);

        Task<bool> FileExists(string filePath);

        Task<Stream> GetFile(string filePath);

        Task<string> GetFileUrl(string filePath);

        Task SaveFile(string filePath, Stream contentStream);
    }
}
