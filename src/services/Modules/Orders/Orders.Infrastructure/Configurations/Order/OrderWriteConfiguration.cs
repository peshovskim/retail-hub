using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderEntity = Orders.Domain.Order.Domain.Order;

namespace Orders.Infrastructure.Configurations.Order;

internal sealed class OrderWriteConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("Order", "orders");

        builder.HasKey(o => o.Id);

        builder.HasQueryFilter(o => o.DeletedOn == null);

        builder.Property(o => o.Id).ValueGeneratedNever();

        builder.Property(o => o.CreatedOn).HasColumnType("datetime2(0)").IsRequired();

        builder.Property(o => o.DeletedOn).HasColumnType("datetime2(0)");

        builder.Property(o => o.UserId);

        builder.Property(o => o.Status).HasMaxLength(64).IsRequired();

        builder.Property(o => o.CartId);

        builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
    }
}
