using System.ComponentModel.DataAnnotations;
using YouTubeV2.Application.Constants;

namespace YouTubeV2.Application.Model
{
    public class Comment
    {
        public Guid Id { get; init; }

        [MinLength(1)]
        [MaxLength(CommentConstants.commentMaxLength)]
        public string Content { get; init; } = null!;

        [Required]
        public DateTimeOffset CreateDate { get; init; }

        public virtual User Author { get; init; } = null!;
        public virtual Video Video { get; init; } = null!;
        public virtual IReadOnlyCollection<CommentResponse> Responses { get; init; } = null!;
    }
}
