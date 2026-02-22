using WebAPI.Models.Order;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;
using WebAPI.Models.Order.Enums;

namespace WebAPI.Services;

public class OrderService : IOrderService
{
    private readonly IUsersService _usersService;
    private readonly IUsersRepository _usersRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IDishesRepository _dishesRepository;


    public OrderService(
        IUsersService usersService, 
        IUsersRepository usersRepository, 
        IOrderRepository orderRepository,
        IDishesRepository dishesRepository)
    {
        _usersService = usersService;   
        _usersRepository = usersRepository;
        _orderRepository = orderRepository;
        _dishesRepository = dishesRepository;
    }

    public async Task CreateOrderAsync(OrderRequest request)
    {
        var userId = _usersService.GetRequiredUserId();
        var user = await _usersRepository.GetUserById(userId);

        if (user == null)
        {
            throw new ApplicationException($"Пользователя с ID={userId} не существует");
        }

        var createdDate = DateTime.UtcNow;

        var phone = request.Phone;

        var order = new OrderEntity
        {   
            User = user,
            UserId = userId,
            CreatedAt = createdDate,
            ContactPhone = phone,
            DeliveryAddress = request.DeliveryAddress,
            Email = request.Email,
            Comment = request.Comment,
            Status = OrderStatus.InWork,
            PaymentStatus = PaymentStatus.Pending,
            PaymentMethod = request.PaymentMethod,
            Items = new List<OrderItem>(),
            TotalSum = 0
        };


        foreach (var item in request.Items)
        {
            var dish = await _dishesRepository.GetDishByIdAsync(item.DishId);

            if(dish == null)
            {
                throw new ApplicationException($"Блюда с ID={item.DishId} не существует");
            }

            var orderItem = new OrderItem
            {

                Dish = dish,
                DishId = item.DishId,
                Amount = item.Amount,
                UnitPrice  = item.Price,

            };

            order.TotalSum += item.Amount * item.Price;

            order.Items.Add(orderItem);
        }

        await _orderRepository.CreateOrderAsync(order);

    }
}
