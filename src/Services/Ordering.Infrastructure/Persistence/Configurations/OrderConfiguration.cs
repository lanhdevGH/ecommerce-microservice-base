using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Key configuration
        builder.HasKey(o => o.Id);

        builder.Property(x => x.Status)
            .HasDefaultValue(EOrderStatus.New)
            .IsRequired();
    }
}
