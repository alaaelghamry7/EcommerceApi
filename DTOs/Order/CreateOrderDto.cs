using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.DTOs;

public class CreateOrderDto
{
    [Required]
    [MinLength(1, ErrorMessage = "You must add at least one item to the order.")]
    public List<CreateOrderItemDto> Items { get; set; } = new();
}

public class CreateOrderItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; }
}