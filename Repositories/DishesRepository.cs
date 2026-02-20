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

    public async Task<List<DishEntity>> GetAllDishesAsync(DishFilters? filters = null, string? sort = null)
    {
        var query = _dbContext.Dishes.Include(c => c.Author).ThenInclude(a => a.User).AsQueryable();


        if (filters.MinPrice.HasValue)
        {
            query = query.Where(d => d.Price >= filters.MinPrice);
        }

        if (filters.MaxPrice.HasValue)
        {
            query = query.Where(d => d.Price <= filters.MaxPrice);
        }

        if(!string.IsNullOrEmpty(filters.Name))
        {   
            query = query.Where(d => d.Name.Contains(filters.Name));
        }

        if (!string.IsNullOrEmpty(filters.Kitchen))
        {
            query = query.Where(d => d.Kitchen == filters.Kitchen);
        }

        if (!string.IsNullOrEmpty(filters.Category))
        {
            query = query.Where(d => d.Category == filters.Category);
        }

        if (sort != null)
        {
            if (sort == "asc")
            {
                query = query.OrderBy(d => d.Price);
            }
            else if (sort == "desc")
            {
                query = query.OrderByDescending(d => d.Price);
            }

        }

        return await query.ToListAsync();
    }

    public async Task<int> CreateDishAsync(DishEntity request)
    {
        _dbContext.Dishes.Add(request);

        try
        {
            await _dbContext.SaveChangesAsync();

            return request.Id;
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
