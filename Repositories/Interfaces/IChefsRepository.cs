using WebAPI.Models.Chef;

namespace WebAPI.Repositories.Interfaces;

public interface IChefsRepository
{
    Task<List<ChefProfile>> GetAllChefsAsync();

    Task<ChefProfile> GetChefByUserIdAsync(int id);

    Task UpdateChef(ChefProfile newChefProfile);

}
