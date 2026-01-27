using Microsoft.AspNetCore.Mvc;
namespace WebAPI.Controllers;

using WebAPI.Models.User;
using WebAPI.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersService _usersService;

    public UsersController( ILogger<UsersController> logger, IUsersService usersService)
    {
        _logger = logger;
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            _logger.LogInformation("GET-запрос списка всех пользователей");
            var users = await _usersService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            _logger.LogInformation($"Поиск для GET-запроса пользователя с ID: {id}");
            var user = await _usersService.GetUserById(id);

            if (user == null)
            {
                _logger.LogInformation($"Пользователь с id = {id} не найден");
                return NotFound(new { message = "Пользователь не найден" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserById(int id, UpdateUserRequest request)
    {
        try
        {
            _logger.LogInformation($"Поиск для PUT-запроса пользователя с ID: {id}");
            var user = await _usersService.UpdateUserById(id, request);

            if (user == null)
            {
                _logger.LogInformation($"Пользователь с id = {id} не найден");
                return NotFound(new { message = "Пользователь не найден" });
            }

            return Ok(new { message = $"Пользователь с id = {id} успешно обновлён" });
        } catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
