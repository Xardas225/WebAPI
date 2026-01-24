using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Models.User;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _signingKey;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;

        // Создаем ключ для подписи токена
        var secretKey = _configuration["Jwt:SecretKey"];
        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    }

    /// <summary>
    /// Генерация Access токена (действует 15 минут - 2 часа)
    /// </summary>
    public string GenerateAccessToken(User user)
    {
        // Шаг 1: Создаем утверждения (Claims) о пользователе
        // Claims - это пары ключ-значение, которые хранят информацию о пользователе
        var claims = new List<Claim>
        {
            // ClaimTypes - стандартные типы утверждений
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID
            new Claim(ClaimTypes.Email, user.Email), // Email пользователя
            new Claim(ClaimTypes.Name, $"{user.Name} {user.LastName}"), // Полное имя
                
            // JWT стандартные claims
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Уникальный ID токена
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), // Время создания токена
                ClaimValueTypes.Integer64),
                
            // Можно добавить роли (если будет система ролей)
            // new Claim(ClaimTypes.Role, "Admin"),
            // new Claim(ClaimTypes.Role, "User"),
        };

        // Шаг 2: Получаем настройки из конфигурации
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expiryInMinutes = Convert.ToInt32(_configuration["Jwt:AccessTokenExpiryMinutes"] ?? "15");

        // Шаг 3: Создаем учетные данные для подписи
        var credentials = new SigningCredentials(
            _signingKey,
            SecurityAlgorithms.HmacSha256 // Алгоритм подписи
        );

        // Шаг 4: Создаем описание токена
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), // Утверждения
            Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes), // Время жизни
            Issuer = issuer, // Кто выдал токен
            Audience = audience, // Для кого предназначен
            SigningCredentials = credentials // Ключ для подписи
        };

        // Шаг 5: Создаем обработчик токенов
        var tokenHandler = new JwtSecurityTokenHandler();

        // Шаг 6: Создаем токен
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Шаг 7: Преобразуем токен в строку
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Генерация Refresh токена (действует 7 дней)
    /// Refresh токен - длительный токен для обновления access токена
    /// Хранится в базе данных и сравнивается при обновлении
    /// </summary>
    public string GenerateRefreshToken()
    {
        // Шаг 1: Создаем массив случайных байтов
        var randomBytes = new byte[64]; // 64 байта = 512 бит

        // Шаг 2: Используем криптографически безопасный генератор случайных чисел
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // Шаг 3: Конвертируем байты в base64 строку
        // Base64 - это способ кодирования бинарных данных в текст
        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// Валидация токена и получение ClaimsPrincipal
    /// </summary>
    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            // Шаг 1: Создаем обработчик токенов
            var tokenHandler = new JwtSecurityTokenHandler();

            // Шаг 2: Получаем настройки валидации
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            // Шаг 3: Настраиваем параметры валидации
            var validationParameters = new TokenValidationParameters
            {
                // Проверяем ключ подписи
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                // Проверяем издателя (Issuer)
                ValidateIssuer = true,
                ValidIssuer = issuer,

                // Проверяем аудиторию (Audience)
                ValidateAudience = true,
                ValidAudience = audience,

                // Проверяем срок действия
                ValidateLifetime = true,

                // Устанавливаем допустимое расхождение времени
                // (чтобы учесть небольшую разницу во времени между серверами)
                ClockSkew = TimeSpan.FromMinutes(5),

                // Требуем наличия срока действия
                RequireExpirationTime = true,
            };

            // Шаг 4: Пытаемся валидировать токен
            // Если токен валиден - возвращаем ClaimsPrincipal
            // Если нет - метод выбросит исключение
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            // Дополнительная проверка: убеждаемся, что это JWT токен
            if (validatedToken is JwtSecurityToken jwtToken)
            {
                // Проверяем алгоритм подписи
                if (jwtToken.Header.Alg != SecurityAlgorithms.HmacSha256)
                {
                    throw new SecurityTokenException("Invalid token algorithm");
                }
            }

            return principal;
        }
        catch (SecurityTokenExpiredException)
        {
            // Токен истек
            Console.WriteLine("Token expired");
            return null;
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            // Неверная подпись токена
            Console.WriteLine("Invalid token signature");
            return null;
        }
        catch (SecurityTokenInvalidIssuerException)
        {
            // Неверный издатель
            Console.WriteLine("Invalid token issuer");
            return null;
        }
        catch (SecurityTokenInvalidAudienceException)
        {
            // Неверная аудитория
            Console.WriteLine("Invalid token audience");
            return null;
        }
        catch (Exception ex)
        {
            // Любая другая ошибка
            Console.WriteLine($"Token validation error: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Получение UserId из токена
    /// </summary>
    public int GetUserIdFromToken(string token)
    {
        try
        {
            var principal = ValidateToken(token);
            if (principal == null)
                throw new Exception("Invalid token");

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new Exception("User ID not found in token");

            return int.Parse(userIdClaim.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user ID from token: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Получение Email из токена
    /// </summary>
    public string GetEmailFromToken(string token)
    {
        try
        {
            var principal = ValidateToken(token);
            if (principal == null)
                throw new Exception("Invalid token");

            var emailClaim = principal.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
                throw new Exception("Email not found in token");

            return emailClaim.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting email from token: {ex.Message}");
            throw;
        }
    }
}
