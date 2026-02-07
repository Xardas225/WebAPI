using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Models.Auth;
using WebAPI.Models.User;
using WebAPI.Services.Interfaces;
using WebAPI.Models.Chef;
using WebAPI.Models.User.Enums;

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

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        // Проверяем, существует ли пользователь с таким email
        if (await CheckEmailExistsAsync(request.Email))
        {
            throw new ApplicationException("Пользователь с таким email уже существует");
        }

        // Валидация пароля
        if (request.Password != request.ConfirmPassword)
        {
            throw new ApplicationException("Пароли не совпадают");
        }

        // Валидация роли (нельзя зарегистрироваться как админ через API)
        if (request.Role == UserRole.Admin)
        {
            throw new ApplicationException("Недопустимая роль для регистрации");
        }

        // Хэшируем пароль
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Создаем пользователя
        var user = new UserProfile
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            Name = request.Name,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow,
            Role = request.Role,
        };

        if(request.Role == UserRole.Chef)
        {   
            user.ChefProfile = new ChefProfile
            {
                KitchenName = request.KitchenName ?? $"{request.Name}'s Kitchen",
                Description = request.ChefDescription ?? "Home Kitchen",
                IsActive = false, // Не активен до верификации
                User = user
            };
        }

        // Сохраняем в БД
        // Вынести в репозиторий
        _context.Users.Add(user);
        await _context.SaveChangesAsync();


        // Возвращаем ответ
        return new RegisterResponse
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
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

        return new LoginResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            LastName = user.LastName,
            Token = token,
            Role = user.Role,
            TokenExpiry = DateTime.UtcNow.AddHours(2),
            AvatarUrl = user.AvatarUrl
        };
    }


}
