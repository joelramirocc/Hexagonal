using System.Collections.Generic;

namespace Inventory.Domain;

public interface IInventoryRepository
{
    Task<InventoryItem?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);

    Task UpsertAsync(InventoryItem item, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<InventoryItem>> ListAsync(CancellationToken cancellationToken = default);
}
