using EcommerceApi.Application.DTOs;
using EcommerceApi.Application.Interfaces.Repositories;
using EcommerceApi.Application.Interfaces.Services;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<(OrderDto? Order, string? Error)> PlaceOrderAsync(CreateOrderDto createDto)
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
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId);

            if (product == null)
                return (null, $"Product ID {itemDto.ProductId} does not exist.");

            if (product.StockQuantity < itemDto.Quantity)
                return (null, $"Not enough stock for {product.Name}. Available: {product.StockQuantity}");

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
        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

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

        return (responseDto, null);
    }
}
