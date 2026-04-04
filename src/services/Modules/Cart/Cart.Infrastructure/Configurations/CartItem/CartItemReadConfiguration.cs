using CartItemEntity = Cart.Domain.Cart.Domain.CartItem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.Infrastructure.Configurations.CartItem;

internal sealed class CartItemReadConfiguration : IEntityTypeConfiguration<CartItemEntity>
{
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        builder.ToTable("CartItem", "cart");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).ValueGeneratedNever();

        builder.Property(i => i.CartId).IsRequired();

        builder.Property(i => i.ProductId).IsRequired();

        builder.Property(i => i.Quantity).IsRequired();

        builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(i => i.CreatedOn).HasColumnType("datetime2(0)").IsRequired();

        builder.Property(i => i.DeletedOn).HasColumnType("datetime2(0)");

        builder.Property(i => i.UpdatedOn).HasColumnType("datetime2(0)");

        builder.HasOne(i => i.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
