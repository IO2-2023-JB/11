using Microsoft.EntityFrameworkCore;
using YouTubeV2.Application.DTO.CommentsDTO;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Providers;
using YouTubeV2.Application.Services.BlobServices;

namespace YouTubeV2.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly YTContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IBlobImageService _blobImageService;

        public CommentService(YTContext context, IDateTimeProvider dateTimeProvider, IBlobImageService blobImageService)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _blobImageService = blobImageService;
        }

        public async Task AddCommentAsync(string commentContent, User author, Video video, CancellationToken cancellationToken)
        {
            Comment comment = new(commentContent, author, video, _dateTimeProvider.UtcNow);

            _context.Users.Attach(author);
            _context.Videos.Attach(video);
            await _context.Comments.AddAsync(comment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<CommentsDTO> GetAllCommentsAsync(Guid videoId, CancellationToken cancellationToken)
        {
            var comments = await _context
                .Comments
                .Include(comment => comment.Author)
                .Where(comment => comment.Video.Id == videoId)
                .Select(comment => new CommentsDTO.CommentDTO(
                    comment.Id,
                    comment.Author.Id,
                    comment.Content,
                    _blobImageService.GetProfilePicture(comment.Author.Id),
                    comment.Author.UserName!,
                    comment.Responses.Any()))
                .ToArrayAsync(cancellationToken);

            return new CommentsDTO(comments);
        }

        public async Task<Comment?> GetCommentByIdAsync(Guid commentId, CancellationToken cancellationToken) =>
            await _context.Comments.FindAsync(new object[] { commentId }, cancellationToken: cancellationToken);

        public async Task AddCommentResponseAsync(string responseContent, User author, Comment comment, CancellationToken cancellationToken)
        {
            CommentResponse commentResponse = new(responseContent, _dateTimeProvider.UtcNow, author, comment);

            _context.Users.Attach(author);
            _context.Comments.Attach(comment);
            await _context.CommentResponses.AddAsync(commentResponse, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
