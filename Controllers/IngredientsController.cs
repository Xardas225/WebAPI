using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
        
    private readonly IIngredientsService _ingredientsService;

    public IngredientsController(IIngredientsService ingredientsService)
    {
        _ingredientsService = ingredientsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllIngredients()
    {
        try
        {
            var ingredients = await _ingredientsService.GetAllIngredientsAsync();

            return Ok(ingredients);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIngredientById(int id)
    {
        try
        {
            var ingredient = await _ingredientsService.GetIngredientByIdAsync(id);

            return Ok(ingredient);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


}
