using WebAPI.Models.User;

namespace WebAPI.Services.Interfaces;

public interface IUsersService
{
    Task<List<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> GetUserById(int id);
    Task<UserResponse> UpdateUserById(int id, UpdateUserRequest request);
}
