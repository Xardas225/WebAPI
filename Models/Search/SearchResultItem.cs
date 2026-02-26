using Qdrant.Client.Grpc;

namespace WebAPI.Models.Search;

public class SearchResultItem
{
    public PointId Id { get; set; }
    public float Score { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Kitchen { get; set; }
    public DateTime CreatedAt { get; set; }

}
