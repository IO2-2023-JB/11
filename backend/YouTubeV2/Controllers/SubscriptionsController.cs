using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Services;

namespace YouTubeV2.Api.Controllers
{
	[ApiController]
	public class SubscriptionsController : ControllerBase
	{
		private readonly SubscriptionsService _subscriptionsService;

		public SubscriptionsController(SubscriptionsService subscriptionsService)
		{
			_subscriptionsService = subscriptionsService;
		}
		[HttpGet("subscriptions")]
		public async Task<ActionResult<UserSubscriptionListDTO>> GetSubscriptionsAsync([FromQuery][Required] Guid id, CancellationToken cancellationToken)
		{
			return Ok((await _subscriptionsService.GetSubscriptionsAsync(id, cancellationToken)));
		}
        [Authorize(Roles = "Simple")]
        [HttpPost("subscriptions")]
        public async Task<IActionResult> PostSubscriptionsAsync([FromQuery][Required] Guid id, CancellationToken cancellationToken)
        {
            var dupa = HttpContext.Request;
            string jwtToken = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);

            await _subscriptionsService.PostSubscriptionsAsync(id, jwtToken, cancellationToken);
            return Ok();
        }
        [HttpDelete("subscriptions")]
        public async Task<IActionResult> DeleteSubscriptionsAsync([FromQuery][Required] Guid id, CancellationToken cancellationToken)
        {
            string jwtToken = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);

            await _subscriptionsService.DeleteSubscriptionsAsync(id, jwtToken, cancellationToken);
            return Ok();
        }
    }
}