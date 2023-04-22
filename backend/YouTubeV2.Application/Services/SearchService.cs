using Microsoft.AspNetCore.Identity;
using YouTubeV2.Application.DTO.SearchDTOS;
using YouTubeV2.Application.DTO.UserDTOS;
using YouTubeV2.Application.Enums;
using YouTubeV2.Application.Exceptions;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly ISubscriptionService _subscriptionService;

        public SearchService(UserManager<User> userManager, IUserService userService, ISubscriptionService subscriptionService) 
        {
            _userManager = userManager;
            _userService = userService;
            _subscriptionService = subscriptionService;
        }

        public async Task<SearchResultsDto> SearchAsync(string query, SortingDirections sortingDirection,
            SortingTypes sortingType, DateTimeOffset? dateBegin, DateTimeOffset? dateEnd, CancellationToken cancellationToken)
        {
            var users = await SearchForUsersAsync(query, sortingDirection, sortingType, dateBegin, dateEnd, cancellationToken);

            return new SearchResultsDto(users);
        }

        private async Task<IReadOnlyList<UserDto>> SearchForUsersAsync(string query, SortingDirections sortingDirection,
            SortingTypes sortingType, DateTimeOffset? dateBegin, DateTimeOffset? dateEnd, CancellationToken cancellationToken)
        {
            var searchableUsers = await _userManager.GetUsersInRoleAsync(Role.Creator);
            var matchingUsers = searchableUsers.Select(user => user).Where(user => user.UserName!
                .Contains(query, StringComparison.InvariantCultureIgnoreCase)).ToList();

            ClipUsersBasedOnDate(ref matchingUsers, dateBegin, dateEnd);
            SortUsers(ref matchingUsers, sortingDirection, sortingType);

            return matchingUsers.Select(async user =>
                await _userService.GetDTOForUser(user, false, cancellationToken)).Select(task => task.Result).ToList(); 
        }

        private void ClipUsersBasedOnDate(ref List<User> users, DateTimeOffset? dateBegin, DateTimeOffset? dateEnd)
        {
            if (dateBegin > dateEnd)
                throw new BadRequestException("Begin date cannot be bigger than end date");

            if (dateBegin != null)
                users = users.Select(user => user).Where(user => user.CreationDate > dateBegin).ToList();
            if (dateBegin != null)
                users = users.Select(user => user).Where(user => user.CreationDate < dateEnd).ToList();
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
                users = users.OrderBy(x => x.CreationDate).ToList();
            else
                users = users.OrderByDescending(x => x.CreationDate).ToList();
        }

        private void SortUsersPopularity(ref List<User> users, SortingDirections sortingDirection)
        {
            if (sortingDirection == SortingDirections.Ascending)
                users = users.OrderBy(x =>
                    _subscriptionService.GetSubscriptionCountAsync(x.Id).GetAwaiter().GetResult()).ToList();
            else
                users = users.OrderByDescending(x => 
                    _subscriptionService.GetSubscriptionCountAsync(x.Id).GetAwaiter().GetResult()).ToList();
        }
    }
}
