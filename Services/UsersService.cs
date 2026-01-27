using Microsoft.EntityFrameworkCore;
using WebAPI.Models.User;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;
namespace WebAPI.Services;
public class UsersService : IUsersService
{   

    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        var users = await _usersRepository.GetAllUsersAsync();
        
        return users.Select(user => new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Phone = user.Phone
        }).ToList();
    }

    public async Task<UserResponse> GetUserById(int id)
    {
        var user = await _usersRepository.GetUserById(id);
        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Phone = user.Phone
        };

    }

    public async Task<UserResponse> UpdateUserById(int id, UpdateUserRequest request)
    {
        var user = await _usersRepository.GetUserById(id);

        user.Update(request.Email, request.Name, request.LastName, request.Phone);

        await _usersRepository.UpdateUser(user);

        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Phone = user.Phone
        };
    }
}

