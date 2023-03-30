using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

            result = await _userManager.AddToRoleAsync(user, registerDto.userType);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));

            if (registerDto.avatarImage.IsNullOrEmpty()) return;

            var newUser = await _userManager.FindByEmailAsync(registerDto.email);
            byte[] image = Convert.FromBase64String(registerDto.avatarImage);
            await _blobImageService.UploadProfilePictureAsync(image, user.Id, cancellationToken);
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
        private async Task<UserDTO> GetDTOForUser(User user)
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
        public async Task<IEnumerable<UserDTO>> SearchAsync(string query, SortingDirections sortingDirection, 
            SortingTypes sortingType, DateTime dateBegin, DateTime dateEnd, CancellationToken cancellationToken)
        {
            var searchableUsers = await _userManager.GetUsersInRoleAsync(Role.Creator);
            var matchingUsers = searchableUsers.Select(user => user).Where(user => user.UserName.
                Contains(query, StringComparison.InvariantCultureIgnoreCase)).ToList();

            ClipUsersBasedOnDate(ref matchingUsers, dateBegin, dateEnd);
            SortUsers(ref matchingUsers, sortingDirection, sortingType);
            
            List<UserDTO> result = new List<UserDTO>();
            foreach (var user in matchingUsers)
            {
                var userDTO = await GetDTOForUser(user);
                result.Add(userDTO);
            }

            return result;
        }
        private void ClipUsersBasedOnDate(ref List<User> users, DateTime dateBegin, DateTime dateEnd)
        {
            if (dateBegin > dateEnd)
                throw new BadRequestException("Begin date cannot be bigger than end date");

            if (dateBegin != DateTime.MinValue)
                users = users.Select(user => user).Where(user => user.AccountCreationDate > dateBegin).ToList();
            if (dateBegin != DateTime.MinValue)
                users = users.Select(user => user).Where(user => user.AccountCreationDate < dateEnd).ToList();
        }
        private void SortUsers(ref List<User> usres, SortingDirections sortingDirection, SortingTypes sortingType)
        {
            switch (sortingType)
            {
                case SortingTypes.Alphabetical:
                    SortUsersAlphabetical(ref usres, sortingDirection);
                    break;
                case SortingTypes.PublishDate:
                    SortUsersPublish(ref usres, sortingDirection);
                    break;
                case SortingTypes.Popularity:
                    SortUsersPopularity(ref usres, sortingDirection);
                    break;
            }
        }
        private void SortUsersAlphabetical(ref List<User> users, SortingDirections sortingDirection)
        {
            if (sortingDirection == SortingDirections.Ascending)
                users = users.OrderBy(x => x.UserName).ToList();
            else
                users = users.OrderByDescending(x => x.UserName).ToList();
        }
        private void SortUsersPublish(ref List<User> users, SortingDirections sortingDirection)
        {
            if (sortingDirection == SortingDirections.Ascending)
                users = users.OrderBy(x => x.AccountCreationDate).ToList();
            else
                users = users.OrderByDescending(x => x.AccountCreationDate).ToList();
        }
        private void SortUsersPopularity(ref List<User> users, SortingDirections sortingDirection)
        {
            if (sortingDirection == SortingDirections.Ascending)
                users = users.OrderBy(x => x.SubscriptionsCount).ToList();
            else
                users = users.OrderByDescending(x => x.SubscriptionsCount).ToList();
        }
    }
}
