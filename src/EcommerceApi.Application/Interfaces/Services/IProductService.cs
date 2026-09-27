using EcommerceApi.Application.DTOs;

namespace EcommerceApi.Application.Interfaces.Services;

public interface IProductService
{
    Task<PagedResponse<ProductDto>> GetProductsAsync(ProductQueryParameters queryParams);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<(ProductDto? Product, string? Error)> CreateProductAsync(CreateProductDto createDto);
    Task<(bool Success, bool NotFound, string? Error)> UpdateProductAsync(int id, UpdateProductDto updateDto);
    Task<bool> DeleteProductAsync(int id);
}
