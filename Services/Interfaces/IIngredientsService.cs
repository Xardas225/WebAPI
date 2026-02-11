using WebAPI.Models.Dish.Ingredient;

namespace WebAPI.Services.Interfaces;

public interface IIngredientsService
{
    public Task<List<IngredientEntity>> GetAllIngredientsAsync();

    public Task<IngredientEntity> GetIngredientByIdAsync(int id);
}
