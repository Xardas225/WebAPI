using WebAPI.Models.Cart;

namespace WebAPI.Repositories.Interfaces;

public interface ICartRepository
{
    public Task AddItemToCartAsync(CartEntity item);

    public Task<List<CartEntity>> GetItemsFromCartByUserIdAsync(int userId);

    public Task<int> GetCountItemsByUserIdAsync(int userId);

}
