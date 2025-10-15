using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Api.Contracts.Requests;
using Inventory.Application.Models;
using Inventory.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/stores")]
public class StoresController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public StoresController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<StoreInventoryDto>), 200)]
    public async Task<ActionResult<IReadOnlyCollection<StoreInventoryDto>>> GetStores(CancellationToken cancellationToken)
    {
        var stores = await _inventoryService.GetStoresAsync(cancellationToken).ConfigureAwait(false);
        return Ok(stores);
    }

    [HttpGet("{storeId:guid}")]
    [ProducesResponseType(typeof(StoreInventoryDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<StoreInventoryDto>> GetStore(Guid storeId, CancellationToken cancellationToken)
    {
        try
        {
            var store = await _inventoryService.GetStoreAsync(storeId, cancellationToken).ConfigureAwait(false);
            return Ok(store);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(StoreInventoryDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<StoreInventoryDto>> CreateStore(
        [FromBody] CreateStoreRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var storeId = await _inventoryService.CreateStoreAsync(request.Name, cancellationToken).ConfigureAwait(false);
            var store = await _inventoryService.GetStoreAsync(storeId, cancellationToken).ConfigureAwait(false);
            return CreatedAtAction(nameof(GetStore), new { storeId }, store);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{storeId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateStore(
        Guid storeId,
        [FromBody] UpdateStoreRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _inventoryService.UpdateStoreNameAsync(storeId, request.Name, cancellationToken).ConfigureAwait(false);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{storeId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteStore(Guid storeId, CancellationToken cancellationToken)
    {
        try
        {
            await _inventoryService.DeleteStoreAsync(storeId, cancellationToken).ConfigureAwait(false);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
