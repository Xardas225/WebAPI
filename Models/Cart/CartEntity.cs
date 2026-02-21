using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Dish;
using WebAPI.Models.User;

namespace WebAPI.Models.Cart;

[Table("cart_items")]
public class CartEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Информация о пользователе
    [Required]
    public UserProfile User { get; set; }
    
    [Required]
    public int UserId { get; set; }

    // Информация о блюде
    [Required]
    public DishEntity Dish { get; set; }

    [Required]
    public int DishId { get; set; }

    [Required]
    public int Amount { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
