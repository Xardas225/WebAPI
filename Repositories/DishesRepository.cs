using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebAPI.Data;
using WebAPI.Models.Dish;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories;

public class DishesRepository : IDishesRepository
{

    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<IDishesRepository> _logger;

    public DishesRepository(ApplicationDbContext dbContext, ILogger<IDishesRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<DishEntity>> GetAllDishesAsync(DishFilters? filters = null, DishSort? sort = null)
    {
        var query = _dbContext.Dishes.Include(c => c.Author).ThenInclude(a => a.User).AsQueryable();

        if(filters.MinPrice.HasValue)
        {
            query = query.Where(d => d.Price >= filters.MinPrice);
        }

        if (filters.MaxPrice.HasValue)
        {
            query = query.Where(d => d.Price <= filters.MaxPrice);
        }

        return await query.ToListAsync();
    }

    public async Task CreateDishAsync(DishEntity request)
    {
        _dbContext.Dishes.Add(request);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            var errorMessage = ex.Message;
            var innerExceptionMessage = ex.InnerException?.Message;
            var innerInnerExceptionMessage = ex.InnerException?.InnerException?.Message;

            _logger.LogError(ex, "DbUpdateException: {Message}", errorMessage);
            _logger.LogError("InnerException: {InnerMessage}", innerExceptionMessage);
            _logger.LogError("InnerInnerException: {InnerInnerMessage}", innerInnerExceptionMessage);

            throw;
        }
        
    }

}
