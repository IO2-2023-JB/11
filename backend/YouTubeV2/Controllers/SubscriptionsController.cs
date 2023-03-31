using Microsoft.AspNetCore.Mvc;
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
		public async Task<IReadOnlyCollection<SubscriptionDTO>> GetSubscriptionsAsync(CancellationToken cancellationToken)
		{
			//await _userService.RegisterAsync(registerDto, cancellationToken);

			return Ok();
		}
	}
}