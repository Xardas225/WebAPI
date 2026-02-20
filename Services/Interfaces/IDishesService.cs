using WebAPI.Models.Dish;

namespace WebAPI.Services.Interfaces;

public interface IDishesService
{
    public Task<List<DishResponse>> GetAllDishesAsync(DishFilters? filters, DishSort? sort);

    public Task CreateDishAsync(DishRequest request);
}
