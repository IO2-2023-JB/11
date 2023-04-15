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

        public async Task AddComment(string commentContent, User author, Video video)
        {
            Comment comment = new(commentContent, author, video, _dateTimeProvider.UtcNow);

            _context.Users.Attach(author);
            _context.Videos.Attach(video);
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public CommentsDTO GetAllComments(Video video)
        {
            return new CommentsDTO(video.Comments.Select(comment =>
                new CommentsDTO.CommentDTO(
                    comment.Id,
                    comment.Author.Id,
                    comment.Content,
                    _blobImageService.GetProfilePicture(comment.Author.Id),
                    comment.Author.UserName!,
                    comment.Responses.Any())).ToArray());
        }
    }
}
