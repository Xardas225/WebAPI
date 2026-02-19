using Microsoft.Extensions.Caching.Distributed;
using WebAPI.Models.Dish.Ingredient;
using WebAPI.Models.Dish.Kitchens;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;
using System.Text.Json;

namespace WebAPI.Services;

public class IngredientsService : IIngredientsService
{
    private const string ingredientsCacheKey = "ingredientsKey";

    private readonly IIngredientsRepository _ingredientsRepository;
    private readonly IDistributedCache _cache;

    public IngredientsService(IIngredientsRepository ingredientsRepository, IDistributedCache cache)
    {
        _ingredientsRepository = ingredientsRepository;
        _cache = cache;
    }

    public async Task<List<IngredientEntity>> GetAllIngredientsAsync()
    {
        var cachedData = await _cache.GetStringAsync(ingredientsCacheKey);


        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<List<IngredientEntity>>(cachedData);
        }

        var response = await _ingredientsRepository.GetAllIngredients();

        var options = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromHours(1)
        };

        await _cache.SetStringAsync(ingredientsCacheKey, JsonSerializer.Serialize(response), options);

        return response;
    }

    public async Task<IngredientEntity> GetIngredientByIdAsync(int id)
    {
        return await _ingredientsRepository.GetIngredientById(id);
    }

}
