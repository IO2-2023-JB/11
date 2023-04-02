using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using YouTubeV2.Application.Configurations.BlobStorage;

namespace YouTubeV2.Application.Services.AzureServices.BlobServices
{
    public class BlobVideoService : IBlobVideoService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobStorageVideosConfig _blobStorageConfig;

        public BlobVideoService(BlobServiceClient blobServiceClient, IOptions<BlobStorageVideosConfig> blobStorageConfig)
        {
            _blobServiceClient = blobServiceClient;
            _blobStorageConfig = blobStorageConfig.Value;
        }

        public async Task<Stream> GetVideoAsync(string fileName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobStorageConfig.VideosContainerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            if (!(await blobClient.ExistsAsync(cancellationToken)).Value)
                throw new FileNotFoundException($"There is no video with fileName {fileName}");

            Stream videoStream = new MemoryStream();
            await blobClient.DownloadToAsync(videoStream, cancellationToken);
            videoStream.Seek(0, SeekOrigin.Begin);
            return videoStream;
        }

        public async Task UploadVideoAsync(string fileName, Stream videoStream, CancellationToken cancellationToken = default)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobStorageConfig.VideosContainerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(
                videoStream,
                new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = "video/mp4" } },
                cancellationToken);
        }
    }
}
