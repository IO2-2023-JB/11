using System.Security.Claims;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Services.JwtFeatures;

namespace YouTubeV2.Application.Services
{
    public interface IUserService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);

        Task RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);

        Task<User?> GetByIdAsync(string id);

        ClaimsPrincipal? ValidateToken(string token);

        string? GetUserId(IEnumerable<Claim> claims) => JwtHandler.GetUserId(claims);

        string? GetUserRole(IEnumerable<Claim> claims) => JwtHandler.GetUserRole(claims);
    }
}
