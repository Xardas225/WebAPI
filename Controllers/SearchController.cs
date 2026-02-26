using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Search;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{

    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService; 
    }

    [HttpPost]
    public async Task<IActionResult> GetSearchResult(SearchRequest request)
    {
        try
        {
            var response = await _searchService.GetSearchResultAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
