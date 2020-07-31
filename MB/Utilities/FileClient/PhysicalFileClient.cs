using System.IO;
using System.Threading.Tasks;

namespace MB.Utilities
{
    public class PhysicalFileClient : IFileClient
    {
        public async Task DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await Task.CompletedTask;
        }

        public async Task<bool> FileExists(string filePath)
        {
            return await Task.FromResult(File.Exists(filePath));
        }

        public async Task<Stream> GetFile(string filePath)
        {
            // Write fileStream to the memoryStream to ensure the fileStream will be closed
            MemoryStream memoryStream = null;
            if (File.Exists(filePath))
            {
                var fileStream = File.OpenRead(filePath);
                memoryStream = new MemoryStream(new byte[fileStream.Length]);
                await fileStream.CopyToAsync(memoryStream);
                fileStream.Close();
                memoryStream.Seek(0, SeekOrigin.Begin);
            }
            return memoryStream;
        }

        public async Task<string> GetFileUrl(string filePath)
        {
            return await Task.FromResult((string)null);
        }

        public async Task SaveFile(string filePath, Stream contentStream)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var file = new FileStream(filePath, FileMode.CreateNew))
            {
                await contentStream.CopyToAsync(file);
            }
        }
    }
}
