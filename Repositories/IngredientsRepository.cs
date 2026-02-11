using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models.Dish.Ingredient;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories;

public class IngredientsRepository : IIngredientsRepository
{

    private readonly ApplicationDbContext _dbContext;

    public IngredientsRepository(ApplicationDbContext dbcontext)
    {
        _dbContext = dbcontext;
    }

    public async Task<List<IngredientEntity>> GetAllIngredients()
    {
        return await _dbContext.Ingredients.ToListAsync();
    }

    public async Task<IngredientEntity> GetIngredientById(int id)
    {
        return await _dbContext.Ingredients.FirstOrDefaultAsync(ing => ing.Id == id);
    }

}
