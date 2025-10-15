using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Contracts.Requests;

/// <summary>
/// Información para actualizar los datos de una tienda.
/// </summary>
public record UpdateStoreRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;
}
