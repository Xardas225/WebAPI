using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Dish.Ingredient;

[Table("dish_ingredients")]
public class DishIngredientEntity
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int DishId { get; set; }
    [Required]
    public DishEntity Dish { get; set; }
    [Required]
    public int IngredientId { get; set; }
    [Required]
    public IngredientEntity Ingredient { get; set; }
    [Required]
    public int Weight { get; set; }
}
