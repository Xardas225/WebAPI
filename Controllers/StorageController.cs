using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.File;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IStorageService _storageService;

    public UploadController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(FileRecordRequest request)
    {
        try
        {   
            var fileUrl = await _storageService.UploadFileAsync(request);
            return Ok(new { url = fileUrl });
        }
        catch (Exception ex)
        {
            return BadRequest($"Ошибка: {ex.Message}");
        }
    }
}
