using EcommerceApi.Application.DTOs;
using EcommerceApi.Application.Interfaces.Repositories;
using EcommerceApi.Domain.Entities;
using EcommerceApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(ProductQueryParameters queryParams)
    {
        IQueryable<Product> query = _context.Products.Include(p => p.Category);

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

        bool isDescending = queryParams.SortOrder?.ToLower() == "desc";

        query = queryParams.SortBy?.ToLower() switch
        {
            "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "name" => isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            _ => isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Product?> GetByIdWithCategoryAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void Remove(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}
