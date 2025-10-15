using BuildingBlocks.Common;
using Sales.Domain;

namespace Sales.Application;

public sealed class OrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<Order>> CreateOrderAsync(string customerName, decimal total, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = new Order(Guid.NewGuid(), customerName, total);
            await _orderRepository.AddAsync(order, cancellationToken);
            return Result<Order>.Success(order);
        }
        catch (Exception ex)
        {
            return Result<Order>.Failure(ex.Message);
        }
    }

    public Task<IReadOnlyList<Order>> GetOrdersAsync(CancellationToken cancellationToken = default)
    {
        return _orderRepository.GetAllAsync(cancellationToken);
    }
}
