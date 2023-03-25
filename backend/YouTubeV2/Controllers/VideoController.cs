using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YouTubeV2.Application.Services.AzureServices.BlobServices;
using YouTubeV2.Application.Utils;

namespace YouTubeV2.Api.Controllers
{
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IBlobVideoService _blobVideoService;

        public VideoController(IBlobVideoService blobVideoService)
        {
            _blobVideoService = blobVideoService;
        }

        [HttpPost("video/{id:guid}")]
        public async Task<ActionResult<string>> GetVideoAsync(Guid id, [FromHeader] string range, CancellationToken cancellationToken)
        {
            Range processedRange = RangeExtensions.FromString(range);
            byte[] bytes = await _blobVideoService.GetVideoAsync(id.ToString(), processedRange, cancellationToken);
            return Ok(System.Text.Encoding.UTF8.GetString(bytes));
        }
    }
}
