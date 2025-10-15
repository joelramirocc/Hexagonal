using Sales.Application;
using Sales.Domain;
using Sales.Infrastructure;

namespace Sales.Tests;

public class OrderServiceTests
{
    [Fact]
    public async Task CreateOrderAsync_ReturnsSuccess_WhenInputIsValid()
    {
        var repository = new InMemoryOrderRepository();
        var service = new OrderService(repository);

        var result = await service.CreateOrderAsync("Test Customer", 100m);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Test Customer", result.Value!.CustomerName);
        Assert.Equal(100m, result.Value.Total);
    }
}
