namespace WebAPI.Repositories;

using WebAPI.Data;
using WebAPI.Models.File;
using WebAPI.Repositories.Interfaces;

public class StorageRepository : IStorageRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StorageRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UploadFileAsync(FileRecord file)
    {
        _dbContext.Files.Add(file);

        await  _dbContext.SaveChangesAsync();
    }
}
