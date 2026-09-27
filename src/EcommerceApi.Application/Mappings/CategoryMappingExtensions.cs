using EcommerceApi.Application.DTOs;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Application.Mappings;

public static class CategoryMappingExtensions
{
    public static CategoryDto ToDto(this Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description
    };

    public static List<CategoryDto> ToDto(this IEnumerable<Category> categories) =>
        categories.Select(ToDto).ToList();

    public static Category ToEntity(this CreateCategoryDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description
    };
}
