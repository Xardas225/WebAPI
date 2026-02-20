using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Dish;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishesController : ControllerBase
{

    private readonly IDishesService _dishesService;

    public DishesController(IDishesService dishesService)
    {
        _dishesService = dishesService; 
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDishes([FromQuery] DishFilters filters, [FromQuery] DishSort sort)
    {
        try
        {
            var dishes = await _dishesService.GetAllDishesAsync(filters, sort);

            return Ok(dishes);
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateDish(DishRequest request)
    {
        try
        {
            await _dishesService.CreateDishAsync(request);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
