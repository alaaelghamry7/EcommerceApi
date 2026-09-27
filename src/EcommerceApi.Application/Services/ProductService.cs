using EcommerceApi.Application.DTOs;
using EcommerceApi.Application.Interfaces.Repositories;
using EcommerceApi.Application.Interfaces.Services;
using EcommerceApi.Application.Mappings;

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

        var productDtos = items.ToDto();

        return new PagedResponse<ProductDto>(productDtos, totalCount, queryParams.PageNumber, queryParams.PageSize);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdWithCategoryAsync(id);
        return product?.ToDto();
    }

    public async Task<(ProductDto? Product, string? Error)> CreateProductAsync(CreateProductDto createDto)
    {
        var categoryExists = await _categoryRepository.ExistsAsync(createDto.CategoryId);
        if (!categoryExists)
        {
            return (null, $"Category with ID {createDto.CategoryId} does not exist.");
        }

        var product = createDto.ToEntity();

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        var savedProduct = await _productRepository.GetByIdWithCategoryAsync(product.Id);
        return (savedProduct!.ToDto(), null);
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

        product.ApplyUpdate(updateDto);

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
}
