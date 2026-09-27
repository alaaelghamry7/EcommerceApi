using EcommerceApi.Application.DTOs;
using EcommerceApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _categoryService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createDto)
    {
        var categoryDto = await _categoryService.CreateCategoryAsync(createDto);
        return CreatedAtAction(nameof(GetCategories), new { id = categoryDto.Id }, categoryDto);
    }
}
