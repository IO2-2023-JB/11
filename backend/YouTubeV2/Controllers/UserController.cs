using Microsoft.AspNetCore.Mvc;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Services;

namespace YouTubeV2.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            await _userService.RegisterAsync(registerDto, cancellationToken);

            return Ok();
        }

        [HttpGet("user")]
        public async Task<UserDto> GetUserAsync([FromQuery] string id, CancellationToken cancellationToken)
        {
            // This endpoint exists only to check if frontend works. It will need to be raplaced

            return new UserDto(id,
              "john.doe@mail.com",
              "johnny123",
              "John",
              "Doe",
              10,
              "Simple",
              "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
              432230);
        }

        [HttpGet("user/videos")]
        public async Task<VideoListDto> GetVideosAsync([FromQuery] string id, CancellationToken cancellationToken)
        {
            // This endpoint exists only to check if frontend works. It will need to be raplaced

            List<string> tags = new List<string>() { "tag1", "tag2", "tag3" };
            List<VideoMetadataDto> videos = new List<VideoMetadataDto>()
            {
                new VideoMetadataDto("k4l2h342kjh", "Title", "desc", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Mr. Beast", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title2", "desc2", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Pewdiepie", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title3", "desc3", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Idk", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title", "desc", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Mr. Beast", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title2", "desc2", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Pewdiepie", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title3", "desc3", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Idk", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title", "desc", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Mr. Beast", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title2", "desc2", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Pewdiepie", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title3", "desc3", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Idk", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title", "desc", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Mr. Beast", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title2", "desc2", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Pewdiepie", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
                new VideoMetadataDto("k4l2h342kjh", "Title3", "desc3", "https://imageslocal.blob.core.windows.net/useravatars/c850be63-9986-4d57-b13e-1466560ef189",
                "h43il5u435", "Idk", 439870324, tags, "public", "queued", DateTime.Now, DateTime.Now, "43:21"),
            };

            return new VideoListDto(videos);
        }
    }
}
