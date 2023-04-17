using System.Linq.Expressions;
using YouTubeV2.Application.DTO.CommentsDTO;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Application.Services
{
    public interface ICommentService
    {
        Task AddCommentAsync(string commentContent, User author, Video video, CancellationToken cancellationToken);

        Task<CommentsDTO> GetAllCommentsAsync(Guid videoId, CancellationToken cancellationToken);

        Task<Comment?> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Comment, object>>[] includes);

        Task AddCommentResponseAsync(string responseContent, User author, Comment comment, CancellationToken cancellationToken);

        Task RemoveCommentAsync(Comment comment, CancellationToken cancellationToken);

        Task<CommentsDTO> GetAllCommentResponsesAsync(Guid commentId, CancellationToken cancellationToken);
    }
}
