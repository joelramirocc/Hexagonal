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
[Route("api/stores/{storeId:guid}/products")]
public class ProductsController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public ProductsController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ProductDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IReadOnlyCollection<ProductDto>>> GetProducts(
        Guid storeId,
        CancellationToken cancellationToken)
    {
        try
        {
            var products = await _inventoryService.GetProductsAsync(storeId, cancellationToken).ConfigureAwait(false);
            return Ok(products);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{productId:guid}")]
    [ProducesResponseType(typeof(ProductDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ProductDto>> GetProduct(
        Guid storeId,
        Guid productId,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = await _inventoryService.GetProductAsync(storeId, productId, cancellationToken).ConfigureAwait(false);
            return Ok(product);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<ActionResult<ProductDto>> CreateProduct(
        Guid storeId,
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var productId = await _inventoryService.AddProductAsync(
                storeId,
                request.Name,
                request.Sku,
                request.Price,
                request.Quantity,
                cancellationToken).ConfigureAwait(false);

            var product = await _inventoryService.GetProductAsync(storeId, productId, cancellationToken).ConfigureAwait(false);
            return CreatedAtAction(nameof(GetProduct), new { storeId, productId }, product);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{productId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProduct(
        Guid storeId,
        Guid productId,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _inventoryService.UpdateProductAsync(
                storeId,
                productId,
                request.Name,
                request.Price,
                request.Quantity,
                cancellationToken).ConfigureAwait(false);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{productId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteProduct(
        Guid storeId,
        Guid productId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _inventoryService.DeleteProductAsync(storeId, productId, cancellationToken).ConfigureAwait(false);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
