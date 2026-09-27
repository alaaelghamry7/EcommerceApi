namespace EcommerceApi.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }

    // Navigation property: An order has many order items
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}