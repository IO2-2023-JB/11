using Microsoft.AspNetCore.Identity;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Exceptions;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Application.Services
{
    public class SearchService
    {
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;

        public SearchService(UserManager<User> userManager, UserService userService) 
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<SearchResultsDTO> SearchAsync(string query, SortingDirections sortingDirection,
            SortingTypes sortingType, DateTimeOffset dateBegin, DateTimeOffset dateEnd, CancellationToken cancellationToken)
        {
            var users = await SearchForUsersAsync(query, sortingDirection, sortingType, dateBegin, dateEnd, cancellationToken);

            return new SearchResultsDTO(users);
        }

        private async Task<IEnumerable<UserDTO>> SearchForUsersAsync(string query, SortingDirections sortingDirection,
            SortingTypes sortingType, DateTimeOffset dateBegin, DateTimeOffset dateEnd, CancellationToken cancellationToken)
        {
            var searchableUsers = await _userManager.GetUsersInRoleAsync(Role.Creator);
            var matchingUsers = searchableUsers.Select(user => user).Where(user => user.UserName.
                Contains(query, StringComparison.InvariantCultureIgnoreCase)).ToList();

            ClipUsersBasedOnDate(ref matchingUsers, dateBegin, dateEnd);
            SortUsers(ref matchingUsers, sortingDirection, sortingType);

            List<UserDTO> result = new List<UserDTO>();
            foreach (var user in matchingUsers)
            {
                var userDTO = await _userService.GetDTOForUser(user);
                result.Add(userDTO);
            }

            return result;
        }

        private void ClipUsersBasedOnDate(ref List<User> users, DateTimeOffset dateBegin, DateTimeOffset dateEnd)
        {
            if (dateBegin > dateEnd)
                throw new BadRequestException("Begin date cannot be bigger than end date");

            if (dateBegin != DateTimeOffset.MinValue)
                users = users.Select(user => user).Where(user => user.AccountCreationDate > dateBegin).ToList();
            if (dateBegin != DateTimeOffset.MinValue)
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
