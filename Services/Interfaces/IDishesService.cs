using WebAPI.Models.Dish;

namespace WebAPI.Services.Interfaces;

public interface IDishesService
{
    public Task<List<DishResponse>> GetAllDishesAsync(DishFilters? filters, string? sort);

    public Task CreateDishAsync(DishRequest request);
}
