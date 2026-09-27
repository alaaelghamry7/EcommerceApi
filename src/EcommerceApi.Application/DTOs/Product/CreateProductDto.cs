using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.Application.DTOs;

public class CreateProductDto
{
    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 10000.00)]
    public decimal Price { get; set; }

    [Range(0, 1000)]
    public int StockQuantity { get; set; }

    [Required]
    public int CategoryId { get; set; }
}