using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Exceptions;

namespace YouTubeV2.Application.Services
{
    public class SearchService
    {
        private readonly UserService _userService;

        public SearchService(UserService userService) 
        {
            _userService = userService;
        }

        public async Task<SearchResultsDTO> SearchAsync(string query, SortingDirections sortingDirection,
            SortingTypes sortingType, DateTime dateBegin, DateTime dateEnd, CancellationToken cancellationToken)
        {
            if (query.IsNullOrEmpty())
                throw new BadRequestException(new ErrorResponseDTO("Query cannot be empty"));

            var users = await SearchForUsersAsync(query, sortingDirection, sortingType, dateBegin, dateEnd, cancellationToken);

            return new SearchResultsDTO(users);
        }

        private async Task<IEnumerable<UserDTO>> SearchForUsersAsync(string query, SortingDirections sortingDirection,
            SortingTypes sortingType, DateTime dateBegin, DateTime dateEnd, CancellationToken cancellationToken)
        {
            return await _userService.SearchAsync(query, sortingDirection, sortingType, dateBegin, dateEnd, cancellationToken);
        }
        
    }
}
