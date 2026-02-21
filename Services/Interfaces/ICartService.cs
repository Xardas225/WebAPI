using WebAPI.Models.Cart;

namespace WebAPI.Services.Interfaces;

public interface ICartService
{
    public Task AddItemToCartAsync(CartRequest request);
    public Task GetItemsFromCartAsync();

}
