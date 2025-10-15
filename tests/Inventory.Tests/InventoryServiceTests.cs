using System.Linq;
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

    [Fact]
    public async Task ListAsync_ReturnsAllExistingItems()
    {
        var repository = new InMemoryInventoryRepository();
        var service = new InventoryService(repository);

        await service.IncreaseStockAsync("ABC-123", 5);
        await service.IncreaseStockAsync("XYZ-789", 3);

        var items = await service.ListAsync();

        Assert.Equal(2, items.Count);
        Assert.Contains(items, item => item.Sku == "ABC-123" && item.Quantity == 5);
        Assert.Contains(items, item => item.Sku == "XYZ-789" && item.Quantity == 3);
    }
}
