using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Models.Dish;

// TODO: разобраться почему не работает Query sort
public class DishSort
{
    public string? Sort { get; set; }
}

public enum SortEnum
{
    asc = 0, 
    desc = 1
}