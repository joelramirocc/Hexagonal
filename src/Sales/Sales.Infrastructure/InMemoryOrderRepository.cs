using Sales.Domain;

namespace Sales.Infrastructure;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();
    private readonly SemaphoreSlim _gate = new(1, 1);

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);
        await _gate.WaitAsync(cancellationToken);
        try
        {
            _orders.Add(order);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            return _orders.ToList();
        }
        finally
        {
            _gate.Release();
        }
    }
}
