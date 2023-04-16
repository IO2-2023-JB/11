using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading;
using YouTubeV2.Api.Attributes;
using YouTubeV2.Application.Constants;
using YouTubeV2.Application.DTO.CommentsDTO;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Services;
using YouTubeV2.Application.Services.VideoServices;

namespace YouTubeV2.Api.Controllers
{
    [ApiController]
    [Route("comment")]
    public class CommentController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IUserService _userService;
        private readonly ICommentService _commentService;

        public CommentController(IVideoService videoService, IUserService userService, ICommentService commentService)
        {
            _videoService = videoService;
            _userService = userService;
            _commentService = commentService;
        }

        [HttpPost]
        [Roles(Role.Simple, Role.Creator, Role.Administrator)]
        [Consumes("text/plain")]
        public async Task<ActionResult> AddCommentAsync([FromQuery] Guid id, [FromBody] string commentContent, CancellationToken cancellationToken)
        {
            if (commentContent.Length > CommentConstants.commentMaxLength) return BadRequest($"Comment must be at most {CommentConstants.commentMaxLength} character long");

            Video? video = await _videoService.GetVideoByIdAsync(id, cancellationToken);
            if (video == null) return NotFound($"Video with id {id} you want to comment not found");

            string authorId = GetUserId();
            User? author = await _userService.GetByIdAsync(authorId);
            if (author == null) return NotFound($"User with id {authorId} not found");

            await _commentService.AddCommentAsync(commentContent, author, video, cancellationToken);

            return Ok();
        }

        [HttpGet]
        [Roles(Role.Simple, Role.Creator, Role.Administrator)]
        public async Task<ActionResult<CommentsDTO>> GetCommentsAsync([FromQuery] Guid id, CancellationToken cancellationToken) =>
            await _commentService.GetAllCommentsAsync(id, cancellationToken);

        [HttpPost("response")]
        [Roles(Role.Simple, Role.Creator, Role.Administrator)]
        [Consumes("text/plain")]
        public async Task<ActionResult> AddCommentResponseAsync([FromQuery] Guid id, [FromBody] string responseContent, CancellationToken cancellationToken)
        {
            if (responseContent.Length > CommentConstants.commentMaxLength) return BadRequest($"Comment must be at most {CommentConstants.commentMaxLength} character long");

            Comment? comment = await _commentService.GetCommentByIdAsync(id, cancellationToken);
            if (comment == null) return NotFound($"Comment with id {id} you want to comment not found");

            string authorId = GetUserId();
            User? author = await _userService.GetByIdAsync(authorId);
            if (author == null) return NotFound($"User with id {authorId} not found");

            await _commentService.AddCommentResponseAsync(responseContent, author, comment, cancellationToken);

            return Ok();
        }

        private string GetUserId() => User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
    }
}
