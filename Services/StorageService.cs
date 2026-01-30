namespace WebAPI.Services;

using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;

public class StorageService
{
private readonly IAmazonS3 _s3Client;
private readonly string _bucketName;

public StorageService(IConfiguration configuration)
{
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

public async Task<string> UploadFileAsync(IFormFile file)
{
    if (file == null || file.Length == 0)
        throw new Exception("Файл пустой");

    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

    var transferUtility = new TransferUtility(_s3Client);

    using var stream = file.OpenReadStream();
    await transferUtility.UploadAsync(stream, _bucketName, fileName);

    return $"https://{_bucketName}.s3.storage.selcloud.ru/{fileName}";
}
}
