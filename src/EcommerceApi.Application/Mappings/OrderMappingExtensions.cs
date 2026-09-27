using EcommerceApi.Application.DTOs;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Application.Mappings;

public static class OrderMappingExtensions
{
    public static OrderItemDto ToDto(this OrderItem orderItem, string productName) => new()
    {
        ProductId = orderItem.ProductId,
        ProductName = productName,
        Quantity = orderItem.Quantity,
        UnitPrice = orderItem.UnitPrice
    };

    public static OrderDto ToDto(this Order order, IReadOnlyDictionary<int, string> productNames) => new()
    {
        Id = order.Id,
        OrderDate = order.OrderDate,
        TotalAmount = order.TotalAmount,
        Items = order.OrderItems
            .Select(oi => oi.ToDto(productNames[oi.ProductId]))
            .ToList()
    };
}
