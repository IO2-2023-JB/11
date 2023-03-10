using FluentValidation;
using Microsoft.AspNetCore.Identity;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Exceptions;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Validator;

namespace YouTubeV2.Application.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RegisterDtoValidator _registerDtoValidator;

        public UserService(UserManager<User> userManager, RegisterDtoValidator registerDtoValidator)
        {
            _userManager = userManager;
            _registerDtoValidator = registerDtoValidator;
        }

        public async Task RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            await _registerDtoValidator.ValidateAndThrowAsync(registerDto, cancellationToken);
            var user = new User(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.password);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));

            result = await _userManager.AddToRoleAsync(user, Role.User);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));

            result = await _userManager.AddToRoleAsync(user, Role.Creator);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));
        }
    }
}
