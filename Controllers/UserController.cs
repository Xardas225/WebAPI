using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace WebAPI.Controllers;

using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Data;
using WebAPI.Models.User;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ApplicationDbContext _dbContext;

    public UserController(ApplicationDbContext dbContext, ILogger<UserController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    } 

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            _logger.LogInformation($"Поиск для GET-запроса пользователя с ID: {id}");
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogInformation($"Пользователь с id = {id} не найден");
                return NotFound(new { message = "Пользователь не найден" });
            }

            var response = new UserResponse
            {
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.Phone
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
    {
        try
        {
            _logger.LogInformation($"Поиск для PUT-запроса пользователя с ID: {id}");
            var user = await _dbContext.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogInformation($"Пользователь с id = {id} не найден");
                return NotFound(new { message = "Пользователь не найден" });
            }

            user.Email = request.Email;
            user.Name = request.Name;
            user.LastName = request.LastName;
            user.Phone = request.Phone;
            user.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return Ok(new { message = $"Пользователь с id = {id} успешно обновлён" });
        } catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
