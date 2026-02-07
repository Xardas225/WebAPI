namespace WebAPI.Models.Dish;

public class DishRequest
{
    public int UserId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Category { get; set; }

    public int Price { get; set; }

    public string Currency { get; set; }

    public string Kitchen { get; set; }
}
