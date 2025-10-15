using Inventory.Application;
using Inventory.Infrastructure;

namespace Inventory.Tests;

public class InventoryServiceTests
{
    [Fact]
    public async Task IncreaseStockAsync_CreatesNewItem_WhenSkuIsUnknown()
    {
        var repository = new InMemoryInventoryRepository();
        var service = new InventoryService(repository);

        var result = await service.IncreaseStockAsync("ABC-123", 5);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("ABC-123", result.Value!.Sku);
        Assert.Equal(5, result.Value.Quantity);
    }
}
