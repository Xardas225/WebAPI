using WebAPI.Models.Dish.Ingredient;

namespace WebAPI.Repositories.Interfaces;

public interface IIngredientsRepository
{
    public Task<IngredientEntity> GetIngredientById(int id);

    public Task<List<IngredientEntity>> GetAllIngredients();
}
