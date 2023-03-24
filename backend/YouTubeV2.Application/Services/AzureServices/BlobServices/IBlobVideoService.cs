namespace YouTubeV2.Application.Services.AzureServices.BlobServices
{
    public interface IBlobVideoService
    {
        Task<byte[]> GetVideoAsync(string fileName, Range bytesRange, CancellationToken cancellationToken = default);
    }
}
