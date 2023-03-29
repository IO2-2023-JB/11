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
            var topRoleName = await GetTopRoleForUserAsync(user);

            return new UserDTO(user.Id, user.Email, user.UserName, user.Name, user.Surname, user.AccountBalance,
                topRoleName, imageUri.ToString(), user.SubscriptionsCount);
        }
        private async Task<string> GetTopRoleForUserAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            string topRoleName = userRoles.Contains(Role.Creator, StringComparer.InvariantCultureIgnoreCase) 
                ? Role.Creator : Role.Simple;

            return topRoleName;
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

            await VerifyEmailUniqueAsync(user);
            await VerifyNameUniqueAsync(user);

            user.Email = userDTO.email;
            user.UserName = userDTO.nickname;
            user.Name = userDTO.name;
            user.Surname = userDTO.surname;
            user.AccountBalance = userDTO.accountBalance;
            user.SubscriptionsCount = userDTO.subscriptionsCount;

            var topRoleName = await GetTopRoleForUserAsync(user);

            if (topRoleName != userDTO.userType)
                await EditUserRoleAsync(user, topRoleName);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(error => new ErrorResponseDTO(error.Description)));
        }

        private async Task VerifyEmailUniqueAsync(User user)
        {
            var userFound = await _userManager.FindByEmailAsync(user.Email);
            if (userFound != null && userFound != user)
                throw new BadRequestException(new ErrorResponseDTO("User with this email already exists"));
        }
        private async Task VerifyNameUniqueAsync(User user)
        {
            var userFound = await _userManager.FindByNameAsync(user.UserName);
            if (userFound != null && userFound != user)
                throw new BadRequestException(new ErrorResponseDTO("User with this nickname already exists"));
        }
        private async Task EditUserRoleAsync(User user, string topRole)
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
                throw new BadRequestException(new ErrorResponseDTO("Begin date cannot be bigger than end date"));

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
