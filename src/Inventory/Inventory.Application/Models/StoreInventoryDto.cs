using System;
using System.Collections.Generic;

namespace Inventory.Application.Models;

/// <summary>
/// Representa la información expuesta del inventario de una tienda.
/// </summary>
public record StoreInventoryDto(Guid StoreId, string StoreName, IReadOnlyCollection<ProductDto> Products);
