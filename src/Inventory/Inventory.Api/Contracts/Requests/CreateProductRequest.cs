using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Contracts.Requests;

/// <summary>
/// Información necesaria para crear un producto.
/// </summary>
public record CreateProductRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Sku { get; init; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; init; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; init; }
}
