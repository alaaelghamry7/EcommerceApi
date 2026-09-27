using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(500);

        builder.HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Gadgets and devices" },
            new Category { Id = 2, Name = "Clothing", Description = "Apparel and accessories" }
        );
    }
}
