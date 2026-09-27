using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<bool> ExistsAsync(int id);
    Task AddAsync(Category category);
    Task<bool> SaveChangesAsync();
}
