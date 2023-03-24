using Microsoft.AspNetCore.Mvc;
using System.Text;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Services;
using Newtonsoft.Json;

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
        // TODO after login - auth
        [HttpDelete("/user")]
        public async Task<IActionResult> DeleteAsync([FromQuery] string userID, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(userID, cancellationToken);

            return Ok();
        }

        [HttpGet("/user")]
        public async Task<ActionResult<UserDTO>> GetAsync([FromQuery] string userID, CancellationToken cancellationToken)
        {
            var userDTO = await _userService.GetAsync(userID, cancellationToken);

            return userDTO;
        }
        
    }
}
