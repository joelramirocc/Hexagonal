using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Domain;

namespace Inventory.Infrastructure;

/// <summary>
/// Implementación en memoria del repositorio de tiendas para propósitos de demostración y pruebas.
/// </summary>
public class InMemoryStoreRepository : IStoreRepository, IDisposable
{
    private readonly Dictionary<Guid, Store> _stores = new();
    private readonly SemaphoreSlim _mutex = new(1, 1);
    private bool _disposed;

    public async Task<Store?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            _stores.TryGetValue(id, out var store);
            return store;
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task<IReadOnlyCollection<Store>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            return _stores.Values.ToArray();
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task AddAsync(Store store, CancellationToken cancellationToken = default)
    {
        if (store is null)
        {
            throw new ArgumentNullException(nameof(store));
        }

        await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_stores.ContainsKey(store.Id))
            {
                throw new InvalidOperationException($"Ya existe una tienda con Id {store.Id}.");
            }

            _stores.Add(store.Id, store);
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task UpdateAsync(Store store, CancellationToken cancellationToken = default)
    {
        if (store is null)
        {
            throw new ArgumentNullException(nameof(store));
        }

        await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (!_stores.ContainsKey(store.Id))
            {
                throw new KeyNotFoundException($"No se encontró la tienda con Id {store.Id}.");
            }

            _stores[store.Id] = store;
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (!_stores.Remove(id))
            {
                throw new KeyNotFoundException($"No se encontró la tienda con Id {id}.");
            }
        }
        finally
        {
            _mutex.Release();
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _mutex.Dispose();
        _disposed = true;
    }
}
