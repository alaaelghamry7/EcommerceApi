using EcommerceApi.Application.DTOs;

namespace EcommerceApi.Application.Interfaces.Services;

public interface IOrderService
{
    Task<(OrderDto? Order, string? Error)> PlaceOrderAsync(CreateOrderDto createDto);
}
