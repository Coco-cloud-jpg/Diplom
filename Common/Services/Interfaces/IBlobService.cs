using Azure.Storage.Blobs.Models;

namespace Common.Services.Interfaces
{
    public interface IBlobService
    {
        Task<Stream> GetBlobAsync(string containerName, string blobName);
        Task<IEnumerable<string>> ListBlobAsync(string containerName);
        Task UploadFileBlobAsync(string containerName, string base64, string fileName);
        Task UploadContentBlobAsync(string containerName, string content, string fileName);
    }
}
