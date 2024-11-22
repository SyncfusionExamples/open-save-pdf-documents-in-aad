using Azure.Identity;
using System.IO;
using System.Threading.Tasks;
using System;
using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Configuration;

namespace ControllerPractice
{
    public class FileService
    {
        private readonly ShareClient _shareClient;

        public FileService(IConfiguration configuration)
        {
            string storageAccountName = configuration["AzureFileShare:StorageAccountName"];
            string fileShareName = configuration["AzureFileShare:FileShareName"];
            //var uri = new Uri("https://pdfviewetstorageacc.file.core.windows.net/pdffile");
            var uri = new Uri($"https://{storageAccountName}.file.core.windows.net/{fileShareName}");
            _shareClient = new ShareClient(uri, new DefaultAzureCredential());
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            //var fileClient = _shareClient.GetRootDirectoryClient().GetFileClient(fileName);
            //var download = await fileClient.DownloadAsync();
            //return download.Value.Content;

            try
            {
                // Get the directory client
                var directoryClient = _shareClient.GetRootDirectoryClient();

                // Get the file client
                var fileClient = directoryClient.GetFileClient(fileName);

                // Download the file
                var download = await fileClient.DownloadAsync();

                // Return the file content as a stream
                return download.Value.Content;
            }
            catch (Azure.RequestFailedException ex)
            {
                // Handle specific Azure exceptions
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
