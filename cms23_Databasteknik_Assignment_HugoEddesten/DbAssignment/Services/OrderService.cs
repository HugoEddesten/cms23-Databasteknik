using DbAssignment.Entities;
using DbAssignment.Repositories;

namespace DbAssignment.Services;

public class OrderService
{
    private readonly OrderRepository _orderRepository;

    public OrderService(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task AddOrderAsync(OrderEntity order)
    {
        await _orderRepository.CreateAsync(order);
    }
}
