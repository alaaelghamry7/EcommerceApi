namespace EcommerceApi.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    
    // Foreign Key for Order
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    // Foreign Key for Product
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    // Specific details for this item in this order
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}