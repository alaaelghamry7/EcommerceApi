using EcommerceApi.Application.DTOs;
using EcommerceApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> PlaceOrder(CreateOrderDto createDto)
    {
        var (order, error) = await _orderService.PlaceOrderAsync(createDto);

        if (error != null) return BadRequest(error);

        return Ok(order);
    }
}
