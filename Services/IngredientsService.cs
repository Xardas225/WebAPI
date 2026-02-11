using WebAPI.Models.Dish.Ingredient;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class IngredientsService : IIngredientsService
{
    private readonly IIngredientsRepository _ingredientsRepository;

    public IngredientsService(IIngredientsRepository ingredientsRepository)
    {
        _ingredientsRepository = ingredientsRepository;
    }

    public async Task<List<IngredientEntity>> GetAllIngredientsAsync()
    {
        return await _ingredientsRepository.GetAllIngredients();
    }

    public async Task<IngredientEntity> GetIngredientByIdAsync(int id)
    {
        return await _ingredientsRepository.GetIngredientById(id);
    }

}
