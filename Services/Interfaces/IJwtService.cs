using System.Security.Claims;
using WebAPI.Models.User;

namespace WebAPI.Services.Interfaces; 

public interface IJwtService
{
    // Генерация access токена
    string GenerateAccessToken(User user);

    // Генерация refresh токена
    string GenerateRefreshToken();

    // Валидация токена
    ClaimsPrincipal? ValidateToken(string token);

    // Получение userId из токена
    int GetUserIdFromToken(string token);

    // Получение email из токена
    string GetEmailFromToken(string token);
}
