﻿using System.Security.Claims;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Application.Services
{
    public interface IUserService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);

        Task RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);

        Task<User?> GetByIdAsync(string id);

        public ClaimsPrincipal? ValidateToken(string token);
    }
}
