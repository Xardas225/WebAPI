using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Cart;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase 
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService; 
    }


    [HttpPost]
    public async Task<IActionResult> AddItemToCart(CartRequest request)
    {
        try
        {
            await _cartService.AddItemToCartAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetItems(CartRequest request)
    {
        try
        {
            return Ok();
        }   
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
