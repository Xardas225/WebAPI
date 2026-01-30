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
    }

    public async Task<string> UploadFileAsync(FileRecordRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            throw new Exception("Файл пустой");

        var fileName = Guid.NewGuid() + Path.GetExtension(request.File.FileName);

        var transferUtility = new TransferUtility(_s3Client);

        using var stream = request.File.OpenReadStream();
        await transferUtility.UploadAsync(stream, _bucketName, fileName);

        var fileData = (new FileRecord
        {
            Url = $"https://{_bucketName}.s3.storage.selcloud.ru/{fileName}",
            UserId = request.UserId,
            UploadedAt = DateTime.UtcNow
        });

        await _storageRepository.UploadFileAsync(fileData);

        return $"https://{_bucketName}.s3.storage.selcloud.ru/{fileName}";
    }
}
