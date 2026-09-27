using EcommerceApi.Application.DTOs;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(ProductQueryParameters queryParams);
    Task<Product?> GetByIdWithCategoryAsync(int id);
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    void Remove(Product product);
    Task<bool> SaveChangesAsync();
}
