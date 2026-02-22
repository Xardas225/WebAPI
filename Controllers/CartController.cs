using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Models.Cart;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase 
{
    private readonly ICartService _cartService;
    private readonly ILogger<CartController> _logger;   

    public CartController(ICartService cartService, ILogger<CartController> logger)
    {
        _cartService = cartService;
        _logger = logger;
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
    public async Task<IActionResult> GetCartItemsByUserId()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var cartItems = await _cartService.GetItemsFromCartByUserIdAsync(userId);
            return Ok(cartItems);
        }   
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCountItemsByUserId()
    {   
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            var count = await _cartService.GetCountItemsByUserIdAsync(userId);
            return Ok(count);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFromCart(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            await _cartService.DeleteFromCartAsync(id, userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
