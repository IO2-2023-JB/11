using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using YouTubeV2.Api.Enums;
using YouTubeV2.Application.Constants;
using YouTubeV2.Application.DTO;

namespace YouTubeV2.Application.Model
{
    public class Video
    {
        public Guid Id { get; init; }

        [MinLength(1)]
        [MaxLength(VideoConstants.titleMaxLength)]
        public string Title { get; init; } = null!;

        [MinLength(1)]
        [MaxLength(VideoConstants.descriptionMaxLength)]
        public string Description { get; init; } = null!;

        [Required]
        public Visibility Visibility { get; init; }
        public virtual User User { get; init; } = null!;
        public virtual IReadOnlyCollection<Tag> Tags { get; init; } = null!;

        public static Video FromDTO(VideoMetadataPostDTO videoMetadata, User user)
        {
            return new Video(videoMetadata.title, videoMetadata.description, videoMetadata.visibility, videoMetadata.tags, user);
        }

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
