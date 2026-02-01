namespace WebAPI.Services;

using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using WebAPI.Models.File;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

public class StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _containerId;
    private readonly IStorageRepository _storageRepository;

    public StorageService(IConfiguration configuration, IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;

        var config = new AmazonS3Config
        {
            ServiceURL = configuration["SelectelS3:ServiceURL"],
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(
            configuration["SelectelS3:AccessKey"],
            configuration["SelectelS3:SecretKey"],
            config
        );

        _bucketName = configuration["SelectelS3:BucketName"];
        _containerId = configuration["SelectelS3:ContainerId"];
    }

    // Запись файла в S3-хранилище
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Файл пустой");

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

        var transferUtility = new TransferUtility(_s3Client);

        using var stream = file.OpenReadStream();
        await transferUtility.UploadAsync(stream, _bucketName, fileName);

        return $"https://{_containerId}.selstorage.ru/{fileName}";
    }

    public async Task<string> SetUserAvatar(FileRecordRequest request)
    {
        var fileUrl = await UploadFileAsync(request.File);

        var file = (new FileRecord
        {
            Url = fileUrl,
            UserId = request.UserId,
            UploadedAt = DateTime.UtcNow
        });

        await _storageRepository.UploadFileAsync(file);

        return fileUrl;

    }


}
