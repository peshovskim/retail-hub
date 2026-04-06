using CartItemEntity = Cart.Domain.Cart.Domain.CartItem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.Infrastructure.Configurations.CartItem;

internal sealed class CartItemWriteConfiguration : IEntityTypeConfiguration<CartItemEntity>
{
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        builder.ToTable("CartItem", "cart");

        builder.HasKey(i => i.Id);

        builder.HasQueryFilter(i => i.DeletedOn == null);

        builder.Property(i => i.Id).ValueGeneratedOnAdd();

        builder.Property(i => i.Uid).ValueGeneratedNever();

        builder.HasIndex(i => i.Uid).IsUnique();

        builder.HasIndex(i => i.CartId).HasDatabaseName("IX_CartItem_CartId");

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
