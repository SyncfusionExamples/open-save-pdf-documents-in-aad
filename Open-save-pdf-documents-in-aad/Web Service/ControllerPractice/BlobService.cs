using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;

namespace ControllerPractice
{
    public class BlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(string connectionString, string containerName)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task UploadFileAsync(string fileName, Stream fileStream)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            BlobDownloadInfo download = await blobClient.DownloadAsync();

            // Create a memory stream to work with
            var memoryStream = new MemoryStream();
            await download.Content.CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Reset position to start of stream
            return memoryStream;
        }
    }
}
