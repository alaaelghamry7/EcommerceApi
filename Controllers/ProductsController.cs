using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

// GET: api/products?search=phone&categoryId=1&minPrice=100&sortBy=price&sortOrder=desc&pageNumber=1&pageSize=10
[HttpGet]
public async Task<ActionResult<PagedResponse<ProductDto>>> GetProducts([FromQuery] ProductQueryParameters queryParams)
{
    // 1. Build initial IQueryable (Deferred Execution)
    IQueryable<Product> query = _context.Products.Include(p => p.Category);

    // 2. Apply Searching & Filtering
    if (!string.IsNullOrWhiteSpace(queryParams.Search))
    {
        query = query.Where(p => p.Name.Contains(queryParams.Search));
    }

    if (queryParams.CategoryId.HasValue)
    {
        query = query.Where(p => p.CategoryId == queryParams.CategoryId.Value);
    }

    if (queryParams.MinPrice.HasValue)
    {
        query = query.Where(p => p.Price >= queryParams.MinPrice.Value);
    }

    if (queryParams.MaxPrice.HasValue)
    {
        query = query.Where(p => p.Price <= queryParams.MaxPrice.Value);
    }

    // 3. Apply Sorting
    bool isDescending = queryParams.SortOrder?.ToLower() == "desc";

    query = queryParams.SortBy?.ToLower() switch
    {
        "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
        "name" => isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
        _ => isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt)
    };

    // 4. Count total matching items in SQL Server BEFORE pagination
    var totalCount = await query.CountAsync();

    // 5. Apply Pagination (Skip and Take) and Execute Query
    var items = await query
        .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
        .Take(queryParams.PageSize)
        .Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            CreatedAt = p.CreatedAt,
            CategoryId = p.CategoryId,
            CategoryName = p.Category != null ? p.Category.Name : string.Empty
        })
        .ToListAsync();

    // 6. Return response with items and pagination metadata
    return Ok(new PagedResponse<ProductDto>(items, totalCount, queryParams.PageNumber, queryParams.PageSize));
}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return NotFound();

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedAt = product.CreatedAt,
            CategoryId = product.CategoryId,
            CategoryName = product.Category != null ? product.Category.Name : string.Empty
        };

        return Ok(productDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createDto)
    {
        // Verify Category exists before assigning
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == createDto.CategoryId);
        if (!categoryExists)
        {
            return BadRequest($"Category with ID {createDto.CategoryId} does not exist.");
        }

        var product = new Product
        {
            Name = createDto.Name,
            Price = createDto.Price,
            StockQuantity = createDto.StockQuantity,
            CategoryId = createDto.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Reload product with Category details to build response
        await _context.Entry(product).Reference(p => p.Category).LoadAsync();

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedAt = product.CreatedAt,
            CategoryId = product.CategoryId,
            CategoryName = product.Category != null ? product.Category.Name : string.Empty
        };

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updateDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == updateDto.CategoryId);
        if (!categoryExists)
        {
            return BadRequest($"Category with ID {updateDto.CategoryId} does not exist.");
        }

        product.Name = updateDto.Name;
        product.Price = updateDto.Price;
        product.StockQuantity = updateDto.StockQuantity;
        product.CategoryId = updateDto.CategoryId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}