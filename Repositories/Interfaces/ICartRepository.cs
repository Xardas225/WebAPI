using WebAPI.Models.Cart;

namespace WebAPI.Repositories.Interfaces;

public interface ICartRepository
{
    public Task AddItemToCartAsync(CartEntity item);
    public Task GetItemsFromCartAsync();

}
