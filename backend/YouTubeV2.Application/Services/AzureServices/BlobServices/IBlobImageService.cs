namespace YouTubeV2.Application.Services.AzureServices.BlobServices
{
    public interface IBlobImageService
    {
        Task UploadProfilePictureAsync(byte[] bytes, string fileName, CancellationToken cancellationToken = default);
        Uri GetProfilePicture(string fileName);
        Task DeleteProfilePictureAsync(string fileName, CancellationToken cancellationToken = default);
    }
}
