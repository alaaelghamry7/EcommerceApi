using Microsoft.EntityFrameworkCore;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Category constraints
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Description).HasMaxLength(500);
        });

        // Configure Product constraints & relationship
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Name).HasMaxLength(150).IsRequired();
            entity.Property(p => p.Price).HasPrecision(18, 2);

            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

// Configure Order constraints
    modelBuilder.Entity<Order>(entity =>
    {
        entity.Property(o => o.TotalAmount).HasPrecision(18, 2);
    });

    // Configure OrderItem constraints & relationships
    modelBuilder.Entity<OrderItem>(entity =>
    {
        entity.Property(oi => oi.UnitPrice).HasPrecision(18, 2);

        // 1-to-Many: Order -> OrderItems
        entity.HasOne(oi => oi.Order)
              .WithMany(o => o.OrderItems)
              .HasForeignKey(oi => oi.OrderId)
              .OnDelete(DeleteBehavior.Cascade); // If order is deleted, delete its items

        // 1-to-Many: Product -> OrderItems
        entity.HasOne(oi => oi.Product)
              .WithMany(p => p.OrderItems)
              .HasForeignKey(oi => oi.ProductId)
              .OnDelete(DeleteBehavior.Restrict); // Don't allow deleting a product if it's in an order
    });
        // ==========================================
        // DATA SEEDING (Adding Test Data)
        // ==========================================

        // 1. Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Gadgets and devices" },
            new Category { Id = 2, Name = "Clothing", Description = "Apparel and accessories" }
        );

        // 2. Seed Products
        modelBuilder.Entity<Product>().HasData(
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