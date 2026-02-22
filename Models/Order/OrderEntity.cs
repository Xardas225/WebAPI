using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.User;
using WebAPI.Models.Order.Enums;

namespace WebAPI.Models.Order;

[Table("orders")]
public class OrderEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public UserProfile User {  get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
        
    public DateTime? UpdatedAt { get; set; }

    [Required]
    public List<OrderItem> Items { get; set; }

    [Required]
    public OrderStatus Status { get; set; }

    [Required]
    public PaymentStatus PaymentStatus { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    [Required]
    public int TotalSum { get; set; }

    [Required]
    public string DeliveryAddress { get; set; }

    public string? ContactPhone { get; set; }

    public string? Email { get; set; }

    public string? Comment { get; set; }
}
