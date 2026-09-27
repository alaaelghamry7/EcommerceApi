using EcommerceApi.Application.DTOs;
using EcommerceApi.Application.Interfaces.Repositories;
using EcommerceApi.Application.Interfaces.Services;
using EcommerceApi.Application.Mappings;

namespace EcommerceApi.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.ToDto();
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
    {
        var category = createDto.ToEntity();

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return category.ToDto();
    }
}
