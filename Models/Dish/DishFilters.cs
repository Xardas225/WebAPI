namespace WebAPI.Models.Dish;

public class DishFilters
{

    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public string? Name { get; set; }
    public int? KitchenId { get; set; }

}
