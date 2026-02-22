using WebAPI.Models.Cart;

namespace WebAPI.Services.Interfaces;

public interface ICartService
{
    public Task AddItemToCartAsync(CartRequest request);

    public Task<List<CartResponse>> GetItemsFromCartByUserIdAsync(int userId);

    public Task<int> GetCountItemsByUserIdAsync(int userId);

    public Task DeleteFromCartAsync(int itemId, int userId);

}
