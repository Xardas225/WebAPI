using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Chef.Enums;

namespace WebAPI.Models.Chef;

public class ChefProfileRequest
{
    // UserProfile
    [Required]
    public int UserId { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
    [Required]
    public string Phone { get; set; }

    // ChefProfile
    [Required]
    public string KitchenName { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public TimeSpan StartTime { get; set; }
    [Required]
    public TimeSpan EndTime { get; set; }
    [Required]
    public ChefExperience ChefExperience { get; set; }
}
