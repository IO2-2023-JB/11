using YouTubeV2.Application.DTO.VideoMetadataDTOS;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Services.BlobServices;

namespace YouTubeV2.Application.Services.VideoServices
{
    internal static class VideoExtensions
    {
        internal static IQueryable<VideoMetadataDto> ToVideoMetadataDto(this IQueryable<Video> query, IBlobImageService blobImageService)
        {
            return query.Select(video => new VideoMetadataDto(
                video.Id.ToString(),
                video.Title,
                video.Description,
                blobImageService.GetVideoThumbnail(video.Id.ToString()).ToString(),
                video.Author.Id,
                video.Author.UserName!,
                video.ViewCount,
                video.Tags.Select(tag => tag.Value).ToList(),
                video.Visibility.ToString(),
                video.ProcessingProgress.ToString(),
                video.UploadDate.ToUniversalTime().Date,
                video.EditDate.ToUniversalTime().Date,
                video.Duration));
        }
    }
}
