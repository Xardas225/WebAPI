namespace WebAPI.Models.File;

public class FileRecordRequest
{
    public int UserId { get; set; }
    public IFormFile File { get; set; }
}
