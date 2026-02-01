namespace WebAPI.Models.Chef;

using Amazon.Runtime.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using WebAPI.Models.Chef.Enums;
using WebAPI.Models.User;

[Table("chefs")]
public class ChefProfile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public string KitchenName { get; set; }
    public string Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public decimal Rating { get; set; } = 0;

    [Required]
    public int TotalOrders { get; set; } = 0;

    // График работы
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }

    // Связь с пользователем
    public UserProfile User { get; set; }

    [Required]
    // Опыт работы
    public ChefExperience ChefExperience { get; set; }

    public void Update(string email, string name, string lastName, string phone, string kitchenName, string description, TimeSpan startTime, TimeSpan endTime, ChefExperience chefExperience)
    {
        User.Email = email;
        User.Name = name;
        User.LastName = lastName;
        User.Phone = phone;
        KitchenName = kitchenName;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        ChefExperience = chefExperience;
        User.UpdatedAt = DateTime.UtcNow;
    }

}
