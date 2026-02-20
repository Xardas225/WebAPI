using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebAPI.Models.Dish;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishesController : ControllerBase
{

    private readonly IDishesService _dishesService;
    private readonly ILogger<DishesController> _logger;

    public DishesController(IDishesService dishesService, ILogger<DishesController> logger)
    {
        _dishesService = dishesService; 
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDishes([FromQuery] DishFilters filters, [FromQuery] string? sort)
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
