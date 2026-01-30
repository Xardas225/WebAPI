using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.File;

[Table("files")]
public class FileRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public string Url { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
