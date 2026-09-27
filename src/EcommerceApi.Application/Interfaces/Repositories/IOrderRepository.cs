using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<bool> SaveChangesAsync();
}
