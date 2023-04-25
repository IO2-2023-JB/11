using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            var matchingUsers = searchableUsers.Where(user => user.UserName!
                .Contains(query, StringComparison.InvariantCultureIgnoreCase)).AsQueryable();

            ClipUsersBasedOnDate(matchingUsers, dateBegin, dateEnd);
            SortUsers(matchingUsers, sortingDirection, sortingType, cancellationToken);

            var sortedUsers = matchingUsers.ToList();

            var userDtos = new List<UserDto>();
            foreach (var user in sortedUsers)
            {
                var userDto = await _userService.GetDTOForUser(user, false, cancellationToken);
                userDtos.Add(userDto);
            }

            return userDtos;
        }

        private void ClipUsersBasedOnDate(IQueryable<User> users, DateTimeOffset? dateBegin, DateTimeOffset? dateEnd)
        {
            if (dateBegin > dateEnd)
                throw new BadRequestException("Begin date cannot be bigger than end date");

            if (dateBegin != null)
                users.Where(user => user.CreationDate > dateBegin);
            if (dateBegin != null)
                users.Where(user => user.CreationDate < dateEnd);
        }

        private void SortUsers(IQueryable<User> users, SortingDirections sortingDirection, SortingTypes sortingType, 
            CancellationToken cancellationToken = default)
        {
            switch (sortingType)
            {
                case SortingTypes.Alphabetical:
                    SortUsersAlphabetical(users, sortingDirection);
                    break;
                case SortingTypes.PublishDate:
                    SortUsersPublish(users, sortingDirection);
                    break;
                case SortingTypes.Popularity:
                    SortUsersPopularity(users, sortingDirection, cancellationToken);
                    break;
            }
        }

        private void SortUsersAlphabetical(IQueryable<User> users, SortingDirections sortingDirection)
        {
            if (sortingDirection == SortingDirections.Ascending)
                users.OrderBy(x => x.UserName);
            else
                users.OrderByDescending(x => x.UserName);
        }

        private void SortUsersPublish(IQueryable<User> users, SortingDirections sortingDirection)
        {
            if (sortingDirection == SortingDirections.Ascending)
                users.OrderBy(x => x.CreationDate);
            else
                users.OrderByDescending(x => x.CreationDate);
        }

        private void SortUsersPopularity(IQueryable<User> users, SortingDirections sortingDirection, 
            CancellationToken cancellationToken = default)
        {
            if (sortingDirection == SortingDirections.Ascending)
                users.OrderBy(x =>
                    _subscriptionService.GetSubscriptionCountAsync(x.Id, cancellationToken).GetAwaiter().GetResult());
            else
                users.OrderByDescending(x => 
                    _subscriptionService.GetSubscriptionCountAsync(x.Id, cancellationToken).GetAwaiter().GetResult());
        }
    }
}
