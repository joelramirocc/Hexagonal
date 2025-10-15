using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Contracts.Requests;

/// <summary>
/// Información para actualizar un producto existente.
/// </summary>
public record UpdateProductRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; init; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; init; }
}
