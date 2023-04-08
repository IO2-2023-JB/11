using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using YouTubeV2.Api.Enums;
using YouTubeV2.Application.Constants;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Enums;

namespace YouTubeV2.Application.Model
{
    public class Video
    {
        public Guid Id { get; init; }

        [MinLength(1)]
        [MaxLength(VideoConstants.titleMaxLength)]
        public string Title { get; init; }

        [MinLength(1)]
        [MaxLength(VideoConstants.descriptionMaxLength)]
        public string Description { get; init; }

        [Required]
        public Visibility Visibility { get; init; }

        [Required]
        [Range(0, int.MaxValue)]
        public int ViewCount { get; init; } = 0;

        [Required]
        public ProcessingProgress ProcessingProgress { get; init; } = ProcessingProgress.MetadataRecordCreater;

        [Required]
        public DateTimeOffset UploadDate { get; init; } = DateTimeOffset.UtcNow;

        [Required]
        public DateTimeOffset EditDate { get; init; } = DateTimeOffset.UtcNow;

        [Required]
        public string Duration { get; init; } = "00:00";


        public virtual User User { get; init; } = null!;
        public virtual IReadOnlyCollection<Tag> Tags { get; init; }

        public Video() { }

        public static Video FromDTO(VideoMetadataPostDTO videoMetadata, User user) => new (videoMetadata.title, videoMetadata.description, videoMetadata.visibility, videoMetadata.tags, user);

        private Video(string title, string description, Visibility visibility, IReadOnlyCollection<string> tags, User user)
        {
            Title = title;
            Description = description;
            Visibility = visibility;
            Tags = tags.Select(tag => new Tag(tag)).ToImmutableList();
            User = user;
        }
    }
}
