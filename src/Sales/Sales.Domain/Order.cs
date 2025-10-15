namespace Sales.Domain;

public sealed class Order
{
    public Order(Guid id, string customerName, decimal total)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new ArgumentException("Customer name must be provided", nameof(customerName));
        }

        if (total < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(total), "Total cannot be negative");
        }

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        CustomerName = customerName.Trim();
        Total = total;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public Guid Id { get; }

    public string CustomerName { get; }

    public decimal Total { get; private set; }

    public DateTime CreatedOnUtc { get; }

    public void UpdateTotal(decimal newTotal)
    {
        if (newTotal < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newTotal), "Total cannot be negative");
        }

        Total = newTotal;
    }
}
