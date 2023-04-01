using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Services;

namespace YouTubeV2.Api.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("/search")]
        public async Task<ActionResult<SearchResultsDTO>> SearchAsync([FromQuery][Required] string query, 
            [FromQuery][Required] SortingTypes sortingCriterion, [FromQuery][Required] SortingDirections sortingType,
            [FromQuery] DateTimeOffset beginDate, [FromQuery] DateTimeOffset endDate, 
            CancellationToken cancellationToken)
        {
            return await _searchService.SearchAsync(query, sortingType, 
                sortingCriterion, beginDate, endDate, cancellationToken);
        }
    }
}
