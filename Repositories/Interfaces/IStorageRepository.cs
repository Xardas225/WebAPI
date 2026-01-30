using WebAPI.Models.File;

namespace WebAPI.Repositories.Interfaces;

public interface IStorageRepository
{
    public Task UploadFileAsync(FileRecord File);
}
