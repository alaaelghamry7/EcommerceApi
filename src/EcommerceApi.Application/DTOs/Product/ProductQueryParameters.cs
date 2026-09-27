namespace EcommerceApi.Application.DTOs;

public class ProductQueryParameters
{
    // Search & Filtering
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    // Sorting
    public string? SortBy { get; set; } // e.g., "price", "name", or "createdat"
    public string? SortOrder { get; set; } = "asc"; // "asc" or "desc"

    // Pagination
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 50 ? 50 : value; // Cap maximum page size at 50
    }
}