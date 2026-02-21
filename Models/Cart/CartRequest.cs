namespace WebAPI.Models.Cart;

public class CartRequest
{

    public int UserId { get; set; }

    public int DishId { get; set; }

    public int Amount { get; set; }

}
