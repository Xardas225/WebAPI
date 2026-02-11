using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Dish.Ingredient;

[Table("ingredients")]
public class IngredientEntity
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Value { get; set; }

    [Required]
    public string Category { get; set; }
}
