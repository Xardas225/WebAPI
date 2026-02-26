using Qdrant.Client;
using Qdrant.Client.Grpc;
using WebAPI.Models.Search;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class SearchService : ISearchService
{

    private readonly IEmbeddingService  _embeddingService;
    private readonly QdrantClient _qdrantClient;
    private readonly ILogger<ISearchService> _logger;

    public SearchService(IEmbeddingService embeddingService, QdrantClient qdrantClient, ILogger<ISearchService> logger)
    {
        _embeddingService = embeddingService;
        _qdrantClient = qdrantClient;
        _logger = logger;
    }


    public async Task<SearchClientResponse> GetSearchResultAsync(SearchRequest request)
    {

        var queryEmbedding = await _embeddingService.GetEmbeddingAsync(request.Query, "query");

        var searchResult = await _qdrantClient.SearchAsync(
            collectionName: "search_index",
            vector: queryEmbedding,
            payloadSelector: true
        );

        var items = searchResult.Select(p => new SearchResultItem
        {
            Id = p.Id,
            Score = p.Score,
            Name = p.Payload["name"].StringValue,
            Description = p.Payload["description"].StringValue,
            Category = p.Payload["category"].StringValue,
            //Price = p.Payload["price"],
            Kitchen = p.Payload["kitchen"].StringValue,
            CreatedAt = DateTime.Parse(p.Payload["created_at"].StringValue)
        }).ToList();

        var response = new SearchClientResponse
        {
            Items = items,
        };

        return response;
    }

}
