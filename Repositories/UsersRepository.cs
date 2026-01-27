using WebAPI.Data;
using WebAPI.Models.User;
using WebAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace WebAPI.Repositories;

public class UsersRepository : IUsersRepository
{

    private readonly ApplicationDbContext _dbContext;

    public UsersRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext; 
    } 

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User> GetUserById(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        return user;
    }

    public async Task UpdateUser(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}
