using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Chef;
using WebAPI.Models.Dish.Ingredient;

namespace WebAPI.Models.Dish;

[Table("dishes")]
public class DishEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Название блюда
    [Required]
    public string Name { get; set; }

    // Описание блюда
    [Required]
    public string Description { get; set; }

    // Категория блюда (суп, паста и т.п.)
    // TODO: Добавить Enum
    [Required]
    public string Category { get; set; }

    // Цена 
    [Required]
    public int Price { get; set; }

    // Валюта 
    // TODO: Добавить Enum
    [Required]
    public string Currency { get; set; }

    // Тип кухни (Европейская, Российская и т.п.) 
    // TODO: Добавить Enum
    public string Kitchen { get; set; }

    // Время создания 
    public DateTime CreatedDate { get; set; }

    // Владелец блюда
    [Required]
    public ChefProfile Author { get; set; }

    public int AuthorId { get; set; }

    [Required]
    public int CookTime { get; set; }

    [Required]
    public ICollection<DishIngredientEntity> Ingredients { get; set; }
}
