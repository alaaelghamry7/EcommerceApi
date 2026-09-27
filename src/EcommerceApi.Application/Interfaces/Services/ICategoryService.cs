using EcommerceApi.Application.DTOs;

namespace EcommerceApi.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto);
}
