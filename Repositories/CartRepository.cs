using WebAPI.Data;
using WebAPI.Models.Cart;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories;

public class CartRepository : ICartRepository
{

    private readonly ApplicationDbContext _dbContext; 

    public CartRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext; 
    }

    public async Task AddItemToCartAsync(CartEntity item)
    {
        await _dbContext.AddAsync(item);
    }


    public Task GetItemsFromCartAsync()
    {
        return Task.CompletedTask;
    }

}
