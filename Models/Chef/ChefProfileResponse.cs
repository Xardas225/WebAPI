using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Chef.Enums;

namespace WebAPI.Models.Chef;

public class ChefProfileResponse
{

    // UserProfile
    public int UserId { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }


    // ChefProfile
    public int ChefId { get; set; }
    public string? KitchenName { get; set; }
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public decimal Rating { get; set; } = 0;

    public int TotalOrders { get; set; } = 0;

    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }

    public ChefExperience ChefExperience { get; set; }
}
