using FluentValidation;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Services.AzureServices.BlobServices;
using YouTubeV2.Application.Validator;

namespace YouTubeV2.Application.Services
{
    public class VideoService : IVideoService
    {
        private readonly YTContext _context;
        private readonly VideoMetadataPostValidator _videoMetadataDtoValidator;
        private readonly IBlobImageService _blobImageService;

        public VideoService(YTContext context ,VideoMetadataPostValidator videoMetadataDtoValidator, IBlobImageService blobImageService)
        {
            _context = context;
            _videoMetadataDtoValidator = videoMetadataDtoValidator;
            _blobImageService = blobImageService;
        }

        public async Task<Guid> AddVideoMetadataAsync(VideoMetadataPostDTO videoMetadata, User user, CancellationToken cancellationToken = default)
        {
            await _videoMetadataDtoValidator.ValidateAndThrowAsync(videoMetadata);
            var video = Video.FromDTO(videoMetadata, user);
            await _context.Videos.AddAsync(video);
            await _context.SaveChangesAsync();
            await _blobImageService.UploadVideoThumbnailAsync(videoMetadata.thumbnail, video.Id.ToString(), cancellationToken);
            return video.Id;
        }
    }
}
