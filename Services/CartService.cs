using WebAPI.Models.Cart;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    private readonly IDishesRepository _dishesRepository;
    private readonly IUsersRepository _usersRepository;

    public CartService(ICartRepository cartRepository, IDishesRepository dishesRepository, IUsersRepository usersRepository)
    {
        _cartRepository = cartRepository;

        _dishesRepository = dishesRepository;
        _usersRepository = usersRepository;
    }

    public async Task AddItemToCartAsync(CartRequest request)
    {
        var createdDate = DateTime.Now;

        var dish = await _dishesRepository.GetDishByIdAsync(request.DishId);

        if (dish == null)
        {
            throw new ApplicationException("Блюдо не найдено в системе");
        }

        var user = await _usersRepository.GetUserById(request.UserId);

        if (user == null)
        {
            throw new ApplicationException($"Пользователя с ID {request.UserId} не существует в системе");
        }

        var cartItem = new CartEntity
        {
            User = user,
            UserId = request.UserId,
            Dish = dish,
            DishId = request.DishId,
            Amount = request.Amount,
            CreatedAt = createdDate
        };

        await _cartRepository.AddItemToCartAsync(cartItem);
    }

    public async Task<List<CartResponse>> GetItemsFromCartByUserIdAsync(int userId)
    {
        var cartItems = await _cartRepository.GetItemsFromCartByUserIdAsync(userId);

        var cartItemsResponse = cartItems.Select(c => 
        new CartResponse
        {   
            Id = c.Id,
            UserId= c.UserId,
            DishId = c.DishId,
            DishName = c.Dish.Name,
            DishDescription = c.Dish.Description,
            DishPrice = c.Dish.Price,
            DishAmount = c.Amount,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return cartItemsResponse;
    }

    public async Task<int> GetCountItemsByUserIdAsync(int userId)
    {
        var count = await _cartRepository.GetCountItemsByUserIdAsync(userId);

        return count;
    }

    public async Task DeleteFromCartAsync(int itemId, int userId)
    {
        await _cartRepository.DeleteFromCartAsync(itemId, userId);
    }

}
