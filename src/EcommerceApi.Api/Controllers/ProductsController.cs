using EcommerceApi.Application.DTOs;
using EcommerceApi.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/products?search=phone&categoryId=1&minPrice=100&sortBy=price&sortOrder=desc&pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PagedResponse<ProductDto>>> GetProducts([FromQuery] ProductQueryParameters queryParams)
    {
        var response = await _productService.GetProductsAsync(queryParams);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createDto)
    {
        var (product, error) = await _productService.CreateProductAsync(createDto);
        if (error != null) return BadRequest(error);

        return CreatedAtAction(nameof(GetProduct), new { id = product!.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updateDto)
    {
        var (success, notFound, error) = await _productService.UpdateProductAsync(id, updateDto);

        if (notFound) return NotFound();
        if (error != null) return BadRequest(error);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
