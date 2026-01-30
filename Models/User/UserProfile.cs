using Microsoft.EntityFrameworkCore.Update.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.User.Enums;
using WebAPI.Models.Chef;
using WebAPI.Models.File;

namespace WebAPI.Models.User;
[Table("users")] // Имя таблицы в MySQL
public class UserProfile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; }

    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Токены
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Роль пользрователя  
    public UserRole Role { get; set; }

    public FileRecord? AvatarUrl { get; set; }

    public void Update(string email,  string name, string lastName, string phone)
    {
        Email = email;
        Name = name;
        LastName = lastName;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }


    public virtual ChefProfile? ChefProfile { get; set; }

}