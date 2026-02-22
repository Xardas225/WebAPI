namespace WebAPI.Models.Cart;

public class CartResponse
{

    public int Id { get; set; }

    public int UserId { get; set; }

    public int DishId { get; set; }

    public string DishName { get; set; }

    public string DishDescription { get; set; }

    public int DishPrice { get; set; }

    public int DishAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

}
