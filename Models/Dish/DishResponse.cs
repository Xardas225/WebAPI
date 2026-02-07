namespace WebAPI.Models.Dish;

public class DishResponse
{
    public int Id { get; set; }

    // Название блюда
    public string Name { get; set; }

    // Описание блюда
    public string Description { get; set; }

    // Категория блюда (суп, паста и т.п.)
    // TODO: Добавить Enum
    public string Category { get; set; }

    // Цена 
    public int Price { get; set; }

    // Валюта 
    // TODO: Добавить Enum
    public string Currency { get; set; }

    // Тип кухни (Европейская, Российская и т.п.) 
    // TODO: Добавить Enum
    public string Kitchen { get; set; }

    // Владелец блюда
    public int AuthorId { get; set; }
    public string AuthorName { get; set; }
}
