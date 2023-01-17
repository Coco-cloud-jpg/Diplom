using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Common.Services.Interfaces;

namespace Common.Services
{
    public class BlobService: IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<Stream> GetBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            return (await blobClient.DownloadContentAsync()).Value.Content.ToStream();
        }
        public async Task<IEnumerable<string>> ListBlobAsync(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var items = new List<string>();

            await foreach(var blobItem in containerClient.GetBlobsAsync())
            {
                items.Add(blobItem.Name);
            }

            return items;
        }
        public async Task UploadFileBlobAsync(string containerName, string base64, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.UploadBlobAsync(fileName, new MemoryStream(Convert.FromBase64String(base64)));
        }
        public async Task UploadContentBlobAsync(string containerName, string content, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
