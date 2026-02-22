using WebAPI.Models.User.Enums;

namespace WebAPI.Models.Auth;
public class LoginResponse
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Token { get; set; }
    public UserRole Role { get; set; }
    public DateTime TokenExpiry { get; set; }
    public string? AvatarUrl { get; set; }
}
