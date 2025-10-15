using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Domain;

/// <summary>
/// Contrato para acceder y persistir información de tiendas.
/// </summary>
public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Store store, CancellationToken cancellationToken = default);

    Task UpdateAsync(Store store, CancellationToken cancellationToken = default);
}
