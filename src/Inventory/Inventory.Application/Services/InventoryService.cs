using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Application.Models;
using Inventory.Domain;

namespace Inventory.Application.Services;

/// <summary>
/// Servicio de aplicación que orquesta las operaciones básicas del inventario.
/// </summary>
public class InventoryService
{
    private readonly IStoreRepository _storeRepository;

    public InventoryService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));
    }

    public async Task<IReadOnlyCollection<StoreInventoryDto>> GetStoresAsync(CancellationToken cancellationToken = default)
    {
        var stores = await _storeRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return stores
            .Select(MapToDto)
            .ToArray();
    }

    public async Task<StoreInventoryDto> GetStoreAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        var store = await GetStoreEntityAsync(storeId, cancellationToken).ConfigureAwait(false)
                    ?? throw new KeyNotFoundException($"No se encontró la tienda con Id {storeId}.");

        return MapToDto(store);
    }

    public async Task<Guid> CreateStoreAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre de la tienda es obligatorio.", nameof(name));
        }

        var store = new Store(Guid.NewGuid(), name);
        await _storeRepository.AddAsync(store, cancellationToken).ConfigureAwait(false);
        return store.Id;
    }

    public async Task UpdateStoreNameAsync(Guid storeId, string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre de la tienda es obligatorio.", nameof(name));
        }

        var store = await GetStoreEntityAsync(storeId, cancellationToken).ConfigureAwait(false)
                    ?? throw new KeyNotFoundException($"No se encontró la tienda con Id {storeId}.");

        store.Rename(name);
        await _storeRepository.UpdateAsync(store, cancellationToken).ConfigureAwait(false);
    }

    public Task DeleteStoreAsync(Guid storeId, CancellationToken cancellationToken = default)
    {
        return _storeRepository.DeleteAsync(storeId, cancellationToken);
    }

    public async Task<Guid> AddProductAsync(
        Guid storeId,
        string name,
        string sku,
        decimal price,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del producto es obligatorio.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("El SKU del producto es obligatorio.", nameof(sku));
        }

        var store = await GetStoreAsync(storeId, cancellationToken).ConfigureAwait(false);
        if (store is null)
        {
            throw new KeyNotFoundException($"No se encontró la tienda con Id {storeId}.");
        }

        var product = new Product(Guid.NewGuid(), name, sku, price, quantity);
        store.AddProduct(product);
        await _storeRepository.UpdateAsync(store, cancellationToken).ConfigureAwait(false);
        return product.Id;
    }

    public async Task<ProductDto> GetProductAsync(
        Guid storeId,
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var store = await GetStoreEntityAsync(storeId, cancellationToken).ConfigureAwait(false)
                    ?? throw new KeyNotFoundException($"No se encontró la tienda con Id {storeId}.");

        var product = store.GetProduct(productId);
        return MapToDto(product);
    }

    public async Task<IReadOnlyCollection<ProductDto>> GetProductsAsync(
        Guid storeId,
        CancellationToken cancellationToken = default)
    {
        var store = await GetStoreEntityAsync(storeId, cancellationToken).ConfigureAwait(false)
                    ?? throw new KeyNotFoundException($"No se encontró la tienda con Id {storeId}.");

        return store.Products
            .Select(MapToDto)
            .ToArray();
    }

    public async Task UpdateProductAsync(
        Guid storeId,
        Guid productId,
        string name,
        decimal price,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del producto es obligatorio.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "El precio no puede ser negativo.");
        }

        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad no puede ser negativa.");
        }

        var store = await GetStoreEntityAsync(storeId, cancellationToken).ConfigureAwait(false)
                    ?? throw new KeyNotFoundException($"No se encontró la tienda con Id {storeId}.");

        var product = store.GetProduct(productId);
        product.Rename(name);
        product.UpdatePrice(price);
        product.SetStock(quantity);
        await _storeRepository.UpdateAsync(store, cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateProductStockAsync(
        Guid storeId,
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad no puede ser negativa.");
        }

        var store = await GetStoreEntityAsync(storeId, cancellationToken).ConfigureAwait(false)
                    ?? throw new KeyNotFoundException($"No se encontró la tienda con Id {storeId}.");

        var product = store.GetProduct(productId);
        product.SetStock(quantity);
        await _storeRepository.UpdateAsync(store, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteProductAsync(
        Guid storeId,
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var store = await GetStoreEntityAsync(storeId, cancellationToken).ConfigureAwait(false)
                    ?? throw new KeyNotFoundException($"No se encontró la tienda con Id {storeId}.");

        store.RemoveProduct(productId);
        await _storeRepository.UpdateAsync(store, cancellationToken).ConfigureAwait(false);
    }

    public async Task<StoreInventoryDto> GetStoreInventoryAsync(
        Guid storeId,
        CancellationToken cancellationToken = default)
    {
        var store = await GetStoreEntityAsync(storeId, cancellationToken).ConfigureAwait(false)
                    ?? throw new KeyNotFoundException($"No se encontró la tienda con Id {storeId}.");

        return MapToDto(store);
    }

    private Task<Store?> GetStoreEntityAsync(Guid storeId, CancellationToken cancellationToken)
    {
        return _storeRepository.GetByIdAsync(storeId, cancellationToken);
    }

    private static StoreInventoryDto MapToDto(Store store)
    {
        var products = store.Products
            .Select(MapToDto)
            .ToArray();

        return new StoreInventoryDto(store.Id, store.Name, products);
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto(product.Id, product.Name, product.Sku, product.Price, product.Quantity);
    }
}
