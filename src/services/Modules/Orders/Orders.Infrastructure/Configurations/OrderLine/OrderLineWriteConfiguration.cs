using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderLineEntity = Orders.Domain.Order.Domain.OrderLine;

namespace Orders.Infrastructure.Configurations.OrderLine;

internal sealed class OrderLineWriteConfiguration : IEntityTypeConfiguration<OrderLineEntity>
{
    public void Configure(EntityTypeBuilder<OrderLineEntity> builder)
    {
        builder.ToTable("OrderLine", "orders");

        builder.HasKey(l => l.Id);

        builder.HasQueryFilter(l => l.DeletedOn == null);

        builder.Property(l => l.Id).ValueGeneratedOnAdd();

        builder.Property(l => l.Uid).ValueGeneratedNever();

        builder.HasIndex(l => l.Uid).IsUnique();

        builder.HasIndex(l => l.OrderId).HasDatabaseName("IX_OrderLine_OrderId");

        builder.Property(l => l.OrderId).IsRequired();

        builder.Property(l => l.ProductId).IsRequired();

        builder.Property(l => l.ProductUid).IsRequired();

        builder.Property(l => l.Quantity).IsRequired();

        builder.Property(l => l.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(l => l.LineTotal).HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(l => l.CreatedOn).HasColumnType("datetime2(0)").IsRequired();

        builder.Property(l => l.DeletedOn).HasColumnType("datetime2(0)");

        builder.HasOne(l => l.Order)
            .WithMany(o => o.Lines)
            .HasForeignKey(l => l.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
