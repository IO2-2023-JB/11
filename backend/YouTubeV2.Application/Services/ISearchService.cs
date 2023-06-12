using YouTubeV2.Application.DTO.SearchDTOS;
using YouTubeV2.Application.Enums;

namespace YouTubeV2.Application.Services
{
    public interface ISearchService
    {
        public Task<SearchResultsDto> SearchAsync(string userId, string query, SortingDirections sortingDirection,
            SortingTypes sortingType, DateTimeOffset? dateBegin, DateTimeOffset? dateEnd, CancellationToken cancellationToken);
    }
}
