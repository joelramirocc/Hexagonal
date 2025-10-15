using System;

namespace Inventory.Application.Models;

/// <summary>
/// Representación simplificada de un producto para exponerlo fuera del dominio.
/// </summary>
public record ProductDto(Guid Id, string Name, string Sku, decimal Price, int Quantity);
