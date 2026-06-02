using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace EventEase.Web.Services;

public class BlobService
{
    private readonly IConfiguration _config;

    public BlobService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<string?> UploadAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null;

        var connectionString = _config["AzureBlobStorage:ConnectionString"];
        var containerName = _config["AzureBlobStorage:ContainerName"];

        var container = new BlobContainerClient(connectionString, containerName);

        await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        var blob = container.GetBlobClient(fileName);

        using var stream = file.OpenReadStream();

        await blob.UploadAsync(
            stream,
            new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                }
            });

        return blob.Uri.AbsoluteUri;
    }
}