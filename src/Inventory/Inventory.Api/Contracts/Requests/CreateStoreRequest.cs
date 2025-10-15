using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.Contracts.Requests;

/// <summary>
/// Información necesaria para crear una tienda.
/// </summary>
public record CreateStoreRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;
}
