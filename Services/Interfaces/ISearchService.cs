using WebAPI.Models.Search;

namespace WebAPI.Services.Interfaces;

public interface ISearchService
{
    public Task<SearchClientResponse> GetSearchResultAsync(SearchRequest request);
}
