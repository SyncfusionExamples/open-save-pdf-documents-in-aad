using Azure.Identity;
using Azure.Storage.Files.Shares;
using System.IO;
using System.Threading.Tasks;
using System;

namespace ControllerPractice
{
    public class FileService1
    {
        private readonly ShareClient _shareClient;

        public FileService1(string storageAccountName, string fileShareName)
        {
            // Construct the URI for the Azure File Share
            var uri = new Uri($"https://{storageAccountName}.file.core.windows.net/{fileShareName}");

            // Use DefaultAzureCredential to authenticate
            _shareClient = new ShareClient(uri, new DefaultAzureCredential());
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            
            // How 
            var fileClient = _shareClient.GetRootDirectoryClient().GetFileClient(fileName);

            // Download the file
            var download = await fileClient.DownloadAsync();
            return download.Value.Content;
        }
    }
}
