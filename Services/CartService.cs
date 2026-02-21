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

    public Task GetItemsFromCartAsync()
    {
        return Task.FromResult(0); 
    }


}
