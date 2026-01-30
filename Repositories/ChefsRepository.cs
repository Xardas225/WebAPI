using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAPI.Controllers;
using WebAPI.Data;
using WebAPI.Models.Chef;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories;

public class ChefsRepository : IChefsRepository
{
    private readonly ILogger<ChefsRepository> _logger;
    private readonly ApplicationDbContext _dbContext;

    public ChefsRepository(ApplicationDbContext dbContext, ILogger<ChefsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<ChefProfile>> GetAllChefsAsync()
    {
        var chefs = await _dbContext.Chefs.Include(c => c.User).ToListAsync();
        
        var chefsJson = JsonSerializer.Serialize(chefs, new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });

        _logger.LogInformation("Выборка шеф-поваров из репозитория:\n{chefsJson}", chefsJson);

        return chefs;
    }
}
