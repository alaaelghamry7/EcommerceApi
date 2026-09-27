namespace EcommerceApi.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; }

    public int CategoryId { get; set; } // Foreign Key property
    public string CategoryName { get; set; } = string.Empty; // Flattens relational data cleanly
}