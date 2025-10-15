using System.Collections.Generic;
using Inventory.Application;
using Inventory.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class InventoryController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public InventoryController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public sealed record AdjustInventoryRequest(string Sku, int Amount);

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InventoryItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var items = await _inventoryService.ListAsync(cancellationToken);
        return Ok(items);
    }

    [HttpPost("increase")]
    [ProducesResponseType(typeof(InventoryItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InventoryItem>> IncreaseAsync([FromBody] AdjustInventoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _inventoryService.IncreaseStockAsync(request.Sku, request.Amount, cancellationToken);
        if (!result.IsSuccess || result.Value is null)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("reduce")]
    [ProducesResponseType(typeof(InventoryItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InventoryItem>> ReduceAsync([FromBody] AdjustInventoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _inventoryService.ReduceStockAsync(request.Sku, request.Amount, cancellationToken);
        if (!result.IsSuccess || result.Value is null)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
