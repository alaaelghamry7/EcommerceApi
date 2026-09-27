using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> PlaceOrder(CreateOrderDto createDto)
    {
        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            TotalAmount = 0
        };

        var productNames = new Dictionary<int, string>();

        foreach (var itemDto in createDto.Items)
        {
            // 1. Fetch the product from database securely
            var product = await _context.Products.FindAsync(itemDto.ProductId);

            if (product == null)
                return BadRequest($"Product ID {itemDto.ProductId} does not exist.");

            if (product.StockQuantity < itemDto.Quantity)
                return BadRequest($"Not enough stock for {product.Name}. Available: {product.StockQuantity}");

            // 2. Create the OrderItem
            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price // Securely set price from DB, not from user
            };

            // 3. Deduct stock and add to total amount
            product.StockQuantity -= itemDto.Quantity;
            order.TotalAmount += (orderItem.Quantity * orderItem.UnitPrice);

            order.OrderItems.Add(orderItem);
            productNames[product.Id] = product.Name;
        }

        // 4. Save everything to the database
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // 5. Build the Response DTO
        var responseDto = new OrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                ProductName = productNames[oi.ProductId],
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList()
        };

        return Ok(responseDto);
    }
}