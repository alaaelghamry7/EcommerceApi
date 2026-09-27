using EcommerceApi.Application.DTOs;
using EcommerceApi.Application.Interfaces.Repositories;
using EcommerceApi.Application.Interfaces.Services;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResponse<ProductDto>> GetProductsAsync(ProductQueryParameters queryParams)
    {
        var (items, totalCount) = await _productRepository.GetPagedAsync(queryParams);

        var productDtos = items.Select(MapToDto).ToList();

        return new PagedResponse<ProductDto>(productDtos, totalCount, queryParams.PageNumber, queryParams.PageSize);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdWithCategoryAsync(id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<(ProductDto? Product, string? Error)> CreateProductAsync(CreateProductDto createDto)
    {
        var categoryExists = await _categoryRepository.ExistsAsync(createDto.CategoryId);
        if (!categoryExists)
        {
            return (null, $"Category with ID {createDto.CategoryId} does not exist.");
        }

        var product = new Product
        {
            Name = createDto.Name,
            Price = createDto.Price,
            StockQuantity = createDto.StockQuantity,
            CategoryId = createDto.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        var savedProduct = await _productRepository.GetByIdWithCategoryAsync(product.Id);
        return (MapToDto(savedProduct!), null);
    }

    public async Task<(bool Success, bool NotFound, string? Error)> UpdateProductAsync(int id, UpdateProductDto updateDto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return (false, true, null);

        var categoryExists = await _categoryRepository.ExistsAsync(updateDto.CategoryId);
        if (!categoryExists)
        {
            return (false, false, $"Category with ID {updateDto.CategoryId} does not exist.");
        }

        product.Name = updateDto.Name;
        product.Price = updateDto.Price;
        product.StockQuantity = updateDto.StockQuantity;
        product.CategoryId = updateDto.CategoryId;

        await _productRepository.SaveChangesAsync();
        return (true, false, null);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return false;

        _productRepository.Remove(product);
        await _productRepository.SaveChangesAsync();
        return true;
    }

    private static ProductDto MapToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        StockQuantity = product.StockQuantity,
        CreatedAt = product.CreatedAt,
        CategoryId = product.CategoryId,
        CategoryName = product.Category != null ? product.Category.Name : string.Empty
    };
}
