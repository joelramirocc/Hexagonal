using Microsoft.AspNetCore.Mvc;
using Sales.Application;
using Sales.Domain;

namespace Sales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Order>>> GetAsync(CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetOrdersAsync(cancellationToken);
        return Ok(orders);
    }

    public sealed record CreateOrderRequest(string CustomerName, decimal Total);

    [HttpPost]
    [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Order>> PostAsync([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _orderService.CreateOrderAsync(request.CustomerName, request.Total, cancellationToken);
        if (!result.IsSuccess || result.Value is null)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetAsync), new { id = result.Value.Id }, result.Value);
    }
}
