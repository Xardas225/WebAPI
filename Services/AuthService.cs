using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models.User;
using WebAPI.Models.Auth;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(ApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<bool> CheckEmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Проверяем, существует ли пользователь с таким email
        if (await CheckEmailExistsAsync(request.Email))
        {
            throw new ApplicationException("Пользователь с таким email уже существует");
        }

        // Валидация пароля (если не использовали DataAnnotations)
        if (request.Password != request.ConfirmPassword)
        {
            throw new ApplicationException("Пароли не совпадают");
        }

        // Хэшируем пароль
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Создаем пользователя
        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            Name = request.Name,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow,
        };

        // Сохраняем в БД
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Генерируем токен
        var token = _jwtService.GenerateAccessToken(user);

        // Возвращаем ответ
        return new AuthResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Token = token,
            TokenExpiry = DateTime.UtcNow.AddHours(2)
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Ищем пользователя по email
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            throw new ApplicationException("Пользователь не найден");
        }

        // Проверяем пароль
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new ApplicationException("Неверный пароль");
        }

        // Генерируем токен
        var token = _jwtService.GenerateAccessToken(user);

        return new AuthResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Token = token,
            TokenExpiry = DateTime.UtcNow.AddHours(2)
        };
    }


}
