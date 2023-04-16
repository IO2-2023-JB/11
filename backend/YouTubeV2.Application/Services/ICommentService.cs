using YouTubeV2.Application.DTO.CommentsDTO;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Application.Services
{
    public interface ICommentService
    {
        Task AddCommentAsync(string commentContent, User author, Video video, CancellationToken cancellationToken);

        Task<CommentsDTO> GetAllCommentsAsync(Guid videoId, CancellationToken cancellationToken);

        Task<Comment?> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken);

        Task AddCommentResponseAsync(string responseContent, User author, Comment comment, CancellationToken cancellationToken);
    }
}
