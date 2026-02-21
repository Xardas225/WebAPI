using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Chef;
using WebAPI.Models.Chef.Enums;
using WebAPI.Models.User;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class ChefsService : IChefsService
{
    private readonly IChefsRepository _chefsRepository;
    private readonly IDishesService _dishesService;

    public ChefsService(IChefsRepository chefsRepository, IDishesService dishesService)
    {
        _chefsRepository = chefsRepository;
        _dishesService = dishesService;
    }

    public async Task<List<ChefProfileResponse>> GetAllChefsAsync()
    {
        var chefs = await _chefsRepository.GetAllChefsAsync();

        var response = chefs.Select(chef => MapChefProfileResponse(chef)).ToList();

        return response;
    }

    public async Task<ChefProfileResponse> GetChefByUserIdAsync(int id)
    {
        var chef = await _chefsRepository.GetChefByUserIdAsync(id);

        var dishes = await _dishesService.GetAllDishesByAuthorId(chef.Id);

        var chefResponse = MapChefProfileResponse(chef);

        chefResponse.dishes = dishes;
                    
        return chefResponse;
    }

    public async Task UpdateChefByUserIdAsync(ChefProfileRequest request)
    {
        var chef = await _chefsRepository.GetChefByUserIdAsync(request.UserId);

        if(chef != null)
        {
            chef.Update(request.Email, request.Name, request.LastName, request.Phone, request.KitchenName, request.Description, request.StartTime, request.EndTime, request.ChefExperience);
        }

        await _chefsRepository.UpdateChef(chef);

    }

    public ChefProfileResponse MapChefProfileResponse(ChefProfile chef)
    {
        return new ChefProfileResponse
        {
            // UserProfile
            UserId = chef.User.Id,
            Email = chef.User.Email,
            Phone = chef.User.Phone,
            Name = chef.User.Name,
            LastName = chef.User.LastName,
            AvatarUrl = chef.User.AvatarUrl,

            // ChefProfile
            ChefId = chef.Id,
            KitchenName = chef.KitchenName,
            Description = chef.Description,
            IsActive = chef.IsActive,
            Rating = chef.Rating,
            TotalOrders = chef.TotalOrders,
            StartTime = chef.StartTime,
            EndTime = chef.EndTime,
            ChefExperience = chef.ChefExperience
        };
    }

}
