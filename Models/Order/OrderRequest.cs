using WebAPI.Models.Order.Enums;

namespace WebAPI.Models.Order;

public class OrderRequest
{

    public List<OrderItemRequest> Items { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public string DeliveryAddress { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string? Comment { get; set; }

}
