using YouTubeV2.Application.Model;
using YouTubeV2.Application.Providers;

namespace YouTubeV2.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly YTContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CommentService(YTContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task AddComment(string commentContent, User author, Video video)
        {
            Comment comment = new(commentContent, author, video, _dateTimeProvider.UtcNow);

            _context.Users.Attach(author);
            _context.Videos.Attach(video);
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }
    }
}
