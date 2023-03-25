using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading;
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

            if (registerDto.userType.Equals(Role.Simple, StringComparison.InvariantCultureIgnoreCase)) result = await _userManager.AddToRoleAsync(user, Role.Simple);
            else if (registerDto.userType.Equals(Role.Creator, StringComparison.InvariantCultureIgnoreCase))
            {
                result = await _userManager.AddToRoleAsync(user, Role.Simple);

                if (!result.Succeeded)
                    throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));

                result = await _userManager.AddToRoleAsync(user, Role.Creator);
            }


            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));

            if (registerDto.avatarImage.IsNullOrEmpty()) return;

            var newUser = await _userManager.FindByEmailAsync(registerDto.email);
            byte[] image = Convert.FromBase64String(registerDto.avatarImage);
            await _blobImageService.UploadProfilePictureAsync(image, user.Id, cancellationToken);
        }

        public async Task DeleteAsync(string userID, CancellationToken cancellationToken)
        {
            await _userIDValidator.ValidateAndThrowAsync(userID);

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
            await _userIDValidator.ValidateAndThrowAsync(userID);

            var user = await _userManager.FindByIdAsync(userID);
            var imageUri = _blobImageService.GetProfilePicture(user.Id);

            var userRoles = await _userManager.GetRolesAsync(user);
            string topRole = userRoles.Contains(Role.Creator, StringComparer.InvariantCultureIgnoreCase) ? Role.Creator : Role.Simple;

            UserDTO userDTO = new UserDTO(user.Id, user.Email, user.UserName, user.Name, user.Surname, user.AccountBalance,
                topRole, imageUri.ToString(), user.SubscriptionsCount);

            return userDTO;
        }

        public async Task EditAsync(UserDTO userDTO, CancellationToken cancellationToken)
        {
            await _userIDValidator.ValidateAndThrowAsync(userDTO.id);
            await _userDTOValidator.ValidateAndThrowAsync(userDTO);

            var user = await _userManager.FindByIdAsync(userDTO.id);

            if (!userDTO.avatarImage.IsNullOrEmpty())
            {
                await _blobImageService.DeleteProfilePictureAsync(user.Id, cancellationToken);
                byte[] image = Convert.FromBase64String(userDTO.avatarImage);
                await _blobImageService.UploadProfilePictureAsync(image, user.Id, cancellationToken);
            }

            await VerifyEmailUnique(user);
            await VerifyNameUnique(user);

            user.Email = userDTO.email;
            user.UserName = userDTO.nickname;
            user.Name = userDTO.name;
            user.Surname = userDTO.surname;
            user.AccountBalance = userDTO.accountBalance;
            user.SubscriptionsCount = userDTO.subscriptionsCount;


            var userRoles = await _userManager.GetRolesAsync(user);
            string topRole = userRoles.Contains(Role.Creator, StringComparer.InvariantCultureIgnoreCase) ? Role.Creator : Role.Simple;

            if (topRole != userDTO.userType)
                await EditUserRole(user, topRole);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));
        }

        private async Task VerifyEmailUnique(User user)
        {
            var userFound = await _userManager.FindByEmailAsync(user.Email);
            if (userFound != null && userFound != user)
                throw new BadRequestException(new ErrorResponseDTO("User with this email already exists"));
        }
        private async Task VerifyNameUnique(User user)
        {
            var userFound = await _userManager.FindByNameAsync(user.UserName);
            if (userFound != null && userFound != user)
                throw new BadRequestException(new ErrorResponseDTO("User with this nickname already exists"));
        }
        private async Task EditUserRole(User user, string topRole)
        {
            if (topRole == Role.Simple)
            {
                var result = await _userManager.AddToRoleAsync(user, Role.Creator);
                if (!result.Succeeded)
                    throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));
            }
            else if (topRole == Role.Creator)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, Role.Creator);
                if (!result.Succeeded)
                    throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));
            }
        }
    }
}
