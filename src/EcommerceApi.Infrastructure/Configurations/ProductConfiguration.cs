using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Product
            {
                Id = 1,
                Name = "Smartphone",
                Price = 699.99m,
                StockQuantity = 50,
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 2,
                Name = "Laptop",
                Price = 1299.50m,
                StockQuantity = 20,
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 3,
                Name = "T-Shirt",
                Price = 19.99m,
                StockQuantity = 200,
                CategoryId = 2,
                CreatedAt = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 4,
                Name = "Wireless Headphones",
                Price = 149.99m,
                StockQuantity = 75,
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 1, 4, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 5,
                Name = "Smartwatch",
                Price = 249.99m,
                StockQuantity = 40,
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 6,
                Name = "Tablet",
                Price = 499.00m,
                StockQuantity = 30,
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 1, 6, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 7,
                Name = "Jeans",
                Price = 49.99m,
                StockQuantity = 150,
                CategoryId = 2,
                CreatedAt = new DateTime(2026, 1, 7, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 8,
                Name = "Jacket",
                Price = 89.99m,
                StockQuantity = 60,
                CategoryId = 2,
                CreatedAt = new DateTime(2026, 1, 8, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 9,
                Name = "Sneakers",
                Price = 74.99m,
                StockQuantity = 100,
                CategoryId = 2,
                CreatedAt = new DateTime(2026, 1, 9, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
