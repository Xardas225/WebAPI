using Amazon.Runtime.Internal;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAPI.Models.Dish;
using WebAPI.Models.Dish.Ingredient;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class DishesService : IDishesService
{
    private readonly IDishesRepository _dishesRepository;
    private readonly IChefsRepository _chefsRepository;

    public DishesService(IDishesRepository dishesRepository, IChefsRepository chefsRepository)
    {
        _dishesRepository = dishesRepository; 
        _chefsRepository = chefsRepository;
    }

    public async Task<List<DishResponse>> GetAllDishesAsync()
    {
        var dishes = await _dishesRepository.GetAllDishesAsync();

        var dishesResponse = dishes.Select(dish =>
        new DishResponse
        {
            Id = dish.Id,
            Name = dish.Name,
            Description = dish.Description,
            Category = dish.Category,
            Price = dish.Price,
            Currency = dish.Currency,
            Kitchen = dish.Kitchen,
            AuthorId = dish.Author.UserId,
            AuthorName = dish.Author.User.Name
        }).ToList();


        return dishesResponse;
    }


    public async Task CreateDishAsync(DishRequest request)
    {
        var createdDate = DateTime.Now;
        var chef = await _chefsRepository.GetChefByUserIdAsync(request.UserId);

        if(chef == null)
        {
            throw new ApplicationException("Регистрация блюд доступна только для шефов");
        }

        var dish = new DishEntity
        {
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            Price = request.Price,
            Currency = request.Currency,
            Kitchen = request.Kitchen,
            Author = chef,
            AuthorId = chef.Id,
            CreatedDate = createdDate,
            Ingredients = new List<DishIngredientEntity> ()
        };

        //if (request.Ingredients != null && request.Ingredients.Any())
        //{

        //}


        await _dishesRepository.CreateDishAsync(dish);
    }
}
