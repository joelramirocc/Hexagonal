using BuildingBlocks.Common;
using Inventory.Domain;

namespace Inventory.Application;

public sealed class InventoryService
{
    private readonly IInventoryRepository _repository;

    public InventoryService(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<InventoryItem>> IncreaseStockAsync(string sku, int amount, CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            return Result<InventoryItem>.Failure("Amount must be positive");
        }

        var existing = await _repository.GetBySkuAsync(sku, cancellationToken) ?? new InventoryItem(Guid.Empty, sku, 0);
        existing.AdjustQuantity(amount);
        await _repository.UpsertAsync(existing, cancellationToken);
        return Result<InventoryItem>.Success(existing);
    }

    public async Task<Result<InventoryItem>> ReduceStockAsync(string sku, int amount, CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            return Result<InventoryItem>.Failure("Amount must be positive");
        }

        var existing = await _repository.GetBySkuAsync(sku, cancellationToken);
        if (existing is null)
        {
            return Result<InventoryItem>.Failure("Item not found");
        }

        try
        {
            existing.AdjustQuantity(-amount);
            await _repository.UpsertAsync(existing, cancellationToken);
            return Result<InventoryItem>.Success(existing);
        }
        catch (Exception ex)
        {
            return Result<InventoryItem>.Failure(ex.Message);
        }
    }
}
