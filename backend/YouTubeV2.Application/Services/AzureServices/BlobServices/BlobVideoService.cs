﻿using Azure.Storage.Blobs;
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

        public async Task<byte[]> GetVideo(string fileName, Range bytesRange, CancellationToken cancellationToken = default)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobStorageConfig.VideosContainerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            if (!(await blobClient.ExistsAsync()).Value)
                throw new FileNotFoundException($"There is no video with fileName {fileName}");

            using var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);
            memoryStream.Seek(bytesRange.Start.Value, SeekOrigin.Begin);
            var buffer = new byte[bytesRange.End.Value - bytesRange.Start.Value + 1];
            int bytesRead = await memoryStream.ReadAsync(buffer, cancellationToken);
            return buffer[..bytesRead];
        }
    }
}
