using EcommerceApi.Application.DTOs;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Application.Mappings;

public static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        StockQuantity = product.StockQuantity,
        CreatedAt = product.CreatedAt,
        CategoryId = product.CategoryId,
        CategoryName = product.Category != null ? product.Category.Name : string.Empty
    };

    public static List<ProductDto> ToDto(this IEnumerable<Product> products) =>
        products.Select(ToDto).ToList();

    public static Product ToEntity(this CreateProductDto dto) => new()
    {
        Name = dto.Name,
        Price = dto.Price,
        StockQuantity = dto.StockQuantity,
        CategoryId = dto.CategoryId,
        CreatedAt = DateTime.UtcNow
    };

    public static void ApplyUpdate(this Product product, UpdateProductDto dto)
    {
        product.Name = dto.Name;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;
    }
}
