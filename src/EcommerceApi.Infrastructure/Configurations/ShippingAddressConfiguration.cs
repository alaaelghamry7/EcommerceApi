using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EcommerceApi.Domain.Entities;

namespace EcommerceApi.Infrastructure.Configurations;

public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.Property(s => s.Street).HasMaxLength(200).IsRequired();
        builder.Property(s => s.City).HasMaxLength(100).IsRequired();
        builder.Property(s => s.State).HasMaxLength(100).IsRequired();
        builder.Property(s => s.ZipCode).HasMaxLength(20).IsRequired();
        builder.Property(s => s.Country).HasMaxLength(100).IsRequired();

        // 1-to-1: Order -> ShippingAddress
        builder.HasOne(s => s.Order)
               .WithOne(o => o.ShippingAddress)
               .HasForeignKey<ShippingAddress>(s => s.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
