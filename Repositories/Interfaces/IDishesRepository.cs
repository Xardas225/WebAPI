using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Dish;

namespace WebAPI.Repositories.Interfaces;

public interface IDishesRepository
{
    public Task<List<DishEntity>> GetAllDishesAsync(DishFilters? filters, DishSort? sort);

    public Task CreateDishAsync(DishEntity request);
}
