namespace Sales.Domain;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default);
}
