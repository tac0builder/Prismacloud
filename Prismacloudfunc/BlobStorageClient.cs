using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System.Text;

namespace PrismaCloudFunc
{
    internal class BlobStorageClient
    {
        private readonly ILogger _logger;
        private readonly BlobServiceClient _client;
        private const string _containerName = "data";

        public BlobStorageClient(ILogger logger, string storageUri)
        {
            _logger = logger;
            _client = new BlobServiceClient(new Uri(storageUri), new DefaultAzureCredential());
        }

        private BlobClient GetBlobClient(string filename)
        {
            try
            {
                var container = _client.GetBlobContainerClient(_containerName);
                return container.GetBlobClient(filename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "BlobStorageClient.GetBlobClient");
            }
            return null;
        }

        public bool FileHasBeenModifiedToday(string filename)
        {
            try
            {
                var blobClient = GetBlobClient(filename);
                var prop = blobClient.GetProperties();
                return (prop.Value.LastModified.Month == DateTime.Now.Month &&
                        prop.Value.LastModified.Day == DateTime.Now.Day &&
                        prop.Value.LastModified.Year == DateTime.Now.Year);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "BlobStorageClient.FileExist");
            }
            return false;
        }

        public void UploadData(string data, string filename)
        {
            UploadDataAsync(data, filename).GetAwaiter().GetResult();
        }

        private async Task UploadDataAsync(string data, string filename)
        {
            try
            {
                var bData = Encoding.UTF8.GetBytes(data);
                var blobClient = GetBlobClient(filename);
                blobClient.DeleteIfExists();
                var result = await blobClient.UploadAsync(new BinaryData(bData)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "BlobStorageClient.UploadData");
            }
        }
    }
}
