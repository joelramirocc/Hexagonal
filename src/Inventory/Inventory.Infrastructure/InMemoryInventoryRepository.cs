using Inventory.Domain;

namespace Inventory.Infrastructure;

public sealed class InMemoryInventoryRepository : IInventoryRepository
{
    private readonly Dictionary<string, InventoryItem> _items = new(StringComparer.OrdinalIgnoreCase);
    private readonly SemaphoreSlim _gate = new(1, 1);

    public async Task<InventoryItem?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            return null;
        }

        await _gate.WaitAsync(cancellationToken);
        try
        {
            _items.TryGetValue(sku.Trim(), out var item);
            return item;
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task UpsertAsync(InventoryItem item, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(item);
        await _gate.WaitAsync(cancellationToken);
        try
        {
            _items[item.Sku] = item;
        }
        finally
        {
            _gate.Release();
        }
    }
}
