﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using YouTubeV2.Application.Configurations;

namespace YouTubeV2.Application.Services.AzureServices.BlobServices
{
    public class BlobImageService : IBlobImageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobStorageConfig _blobStorageConfig;

        public BlobImageService(BlobServiceClient blobServiceClient, IOptions<BlobStorageConfig> blobStorageConfig)
        {
            _blobServiceClient = blobServiceClient;
            _blobStorageConfig = blobStorageConfig.Value;
        }

        public Uri GetProfilePicture(string fileName) => GetImageUrl(fileName, _blobStorageConfig.UserAvatarsContainerName);

        public async Task UploadProfilePictureAsync(byte[] bytes, string fileName, CancellationToken cancellationToken = default) =>
            await UploadImageAsync(bytes, fileName, _blobStorageConfig.UserAvatarsContainerName, cancellationToken);

        public async Task DeleteProfilePictureAsync(string fileName, CancellationToken cancellationToken = default) =>
            await DeleteImageAsync(fileName, _blobStorageConfig.UserAvatarsContainerName, cancellationToken);

        private Uri GetImageUrl(string fileName, string blobContainerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);

            return blobContainerClient.GetBlobClient(fileName).Uri;
        }

        private async Task UploadImageAsync(byte[] bytes, string fileName, string blobContainerName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
            BlobClient blobClient =  blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(
                new BinaryData(bytes),
                new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = "image/png" } },
                cancellationToken);
        }

        private async Task DeleteImageAsync(string fileName, string blobContainerName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }
    }
}
