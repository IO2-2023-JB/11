using YouTubeV2.Application.DTO.CommentsDTO;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Application.Services
{
    public interface ICommentService
    {
        Task AddComment(string commentContent, User author, Video video);

        Task<CommentsDTO> GetAllComments(Guid videoId, CancellationToken cancellationToken);
    }
}
