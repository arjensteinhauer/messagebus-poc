using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using MB.Utilities.Exceptions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MB.Utilities
{
    public class BlobStorageFileClient : IFileClient
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageFileClient(IConfiguration configuration)
        {
            var section = configuration.GetSection("BlobStorage");
            var blobContainerName = section?.GetValue<string>("ContainerName");
            var blobName = section?.GetValue<string>("Name");
            var blobSasToken = section?.GetValue<string>("SasToken");
            if (string.IsNullOrWhiteSpace(blobContainerName) || string.IsNullOrWhiteSpace(blobName) || string.IsNullOrWhiteSpace(blobSasToken))
            {
                throw new ConfigurationException($"Could not load proper configuration settings from the BlobStorage section.");
            }

            var uriBuilder = new UriBuilder()
            {
                Scheme = "https",
                Host = string.Format("{0}.blob.core.windows.net", blobName),
                Path = blobContainerName,
                Query = blobSasToken
            };

            _blobContainerClient = new BlobContainerClient(uriBuilder.Uri);
        }

        public async Task DeleteFile(string filePath)
        {
            var blob = _blobContainerClient.GetBlobClient(filePath);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<bool> FileExists(string filePath)
        {
            var blob = _blobContainerClient.GetBlobClient(filePath);
            var azureResponse = await blob.ExistsAsync();
            return azureResponse.Value;
        }

        public async Task<Stream> GetFile(string filePath)
        {
            var blob = _blobContainerClient.GetBlobClient(filePath);
            if (!(await FileExists(filePath)))
            {
                return null;
            }

            // Write azure stream to the memoryStream to ensure the steam methods can all be used
            var response = await blob.DownloadAsync();
            Stream memoryStream = new MemoryStream(new byte[response.Value.ContentLength]);
            await response.Value.Content.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public async Task<string> GetFileUrl(string filePath)
        {
            return await Task.FromResult((string)null);
        }

        public async Task SaveFile(string filePath, Stream contentStream)
        {
            var blob = _blobContainerClient.GetBlobClient(filePath);
            await blob.DeleteIfExistsAsync();
            await blob.UploadAsync(contentStream);
        }
    }
}
