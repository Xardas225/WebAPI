using WebAPI.Models.Dish;

namespace WebAPI.Services.Interfaces;

public interface IDishesService
{
    public Task<List<DishResponse>> GetAllDishesAsync();

    public Task CreateDishAsync(DishRequest request);
}
