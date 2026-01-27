using WebAPI.Models.User;

namespace WebAPI.Repositories.Interfaces;

public interface IUsersRepository
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserById(int id);
    Task UpdateUser(User user);
}
