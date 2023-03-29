using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using YouTubeV2.Application.DTO;
using YouTubeV2.Application.Services;

namespace YouTubeV2.Api.Controllers
{
    [AllowAnonymous]
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
            [FromQuery] DateTime beginDate, [FromQuery] DateTime endDate, 
            CancellationToken cancellationToken)
        {
            SearchResultsDTO result = await _searchService.SearchAsync(query, sortingType, 
                sortingCriterion, beginDate, endDate, cancellationToken);

            return result;
        }
    }
}
