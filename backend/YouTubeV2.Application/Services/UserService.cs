using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Exceptions;
using YouTubeV2.Application.Model;
using YouTubeV2.Application.Services.AzureServices.BlobServices;
using YouTubeV2.Application.Validator;

namespace YouTubeV2.Application.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IBlobImageService _blobImageService;
        private readonly RegisterDtoValidator _registerDtoValidator;
        private readonly UserIDValidator _userIDValidator;
        private readonly UserDTOValidator _userDTOValidator;

        public UserService(UserManager<User> userManager, IBlobImageService blobImageService, RegisterDtoValidator registerDtoValidator,
            UserIDValidator userIDValidator, UserDTOValidator userDTOValidator) 
        {
            _userManager = userManager;
            _blobImageService = blobImageService;
            _registerDtoValidator = registerDtoValidator;
            _userIDValidator = userIDValidator;
            _userDTOValidator = userDTOValidator;
        }

        public async Task RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            await _registerDtoValidator.ValidateAndThrowAsync(registerDto, cancellationToken);
            var user = new User(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.password);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));

            result = await _userManager.AddToRoleAsync(user, registerDto.userType);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));

            if (registerDto.avatarImage.IsNullOrEmpty()) return;

            var newUser = await _userManager.FindByEmailAsync(registerDto.email);
            byte[] image = Convert.FromBase64String(registerDto.avatarImage);
            await _blobImageService.UploadProfilePictureAsync(image, newUser.Id, cancellationToken);
        }

        public async Task DeleteAsync(string userID, CancellationToken cancellationToken)
        {
            await _userIDValidator.ValidateAndThrowAsync(userID, cancellationToken);

            var user = await _userManager.FindByIdAsync(userID);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                var roleResult = await _userManager.RemoveFromRoleAsync(user, role);
                if (!roleResult.Succeeded)
                    throw new BadRequestException(roleResult.Errors.Select(error => new ErrorResponseDTO(error.Description)));
            }

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
                throw new BadRequestException(deleteResult.Errors.Select(error => new ErrorResponseDTO(error.Description)));

            await _blobImageService.DeleteProfilePictureAsync(user.Id, cancellationToken);
        }

        public async Task<UserDTO> GetAsync(string userID, CancellationToken cancellationToken)
        {
            await _userIDValidator.ValidateAndThrowAsync(userID, cancellationToken);

            var user = await _userManager.FindByIdAsync(userID);

            return await GetDTOForUser(user);
        }

        public async Task<UserDTO> GetDTOForUser(User user)
        {
            var imageUri = _blobImageService.GetProfilePicture(user.Id);
            var roleName = await GetRoleForUserAsync(user);

            return new UserDTO(new Guid(user.Id), user.Email, user.UserName, user.Name, user.Surname, user.AccountBalance,
                roleName, imageUri.ToString(), user.SubscriptionsCount);
        }

        private async Task<string> GetRoleForUserAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.First();
        }

        public async Task EditAsync(UserDTO userDTO, CancellationToken cancellationToken)
        {
            await _userIDValidator.ValidateAndThrowAsync(userDTO.id.ToString());
            await _userDTOValidator.ValidateAndThrowAsync(userDTO);

            var user = await _userManager.FindByIdAsync(userDTO.id.ToString());

            await _blobImageService.DeleteProfilePictureAsync(user.Id, cancellationToken);
            if (!userDTO.avatarImage.IsNullOrEmpty())
            {
                byte[] image = Convert.FromBase64String(userDTO.avatarImage);
                await _blobImageService.UploadProfilePictureAsync(image, user.Id, cancellationToken);
            }

            await VerifyEmailUniqueAsync(user);
            await VerifyNameUniqueAsync(user);

            user.Email = userDTO.email;
            user.UserName = userDTO.nickname;
            user.Name = userDTO.name;
            user.Surname = userDTO.surname;
            user.AccountBalance = userDTO.accountBalance;
            user.SubscriptionsCount = userDTO.subscriptionsCount;

            var currentRoleName = await GetRoleForUserAsync(user);

            if (currentRoleName != userDTO.userType)
                await SwitchUserRoleAsync(user, currentRoleName, userDTO.userType);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));
        }

        private async Task VerifyEmailUniqueAsync(User user)
        {
            var userFound = await _userManager.FindByEmailAsync(user.Email);
            if (userFound != null && userFound != user)
                throw new BadRequestException("User with this email already exists");
        }

        private async Task VerifyNameUniqueAsync(User user)
        {
            var userFound = await _userManager.FindByNameAsync(user.UserName);
            if (userFound != null && userFound != user)
                throw new BadRequestException("User with this nickname already exists");
        }

        private async Task SwitchUserRoleAsync(User user, string currentRole, string newRole)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, currentRole);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));

            result = await _userManager.AddToRoleAsync(user, newRole);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));
        }
    }
}
