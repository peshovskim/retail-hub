using CartEntity = Cart.Domain.Cart.Domain.Cart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.Infrastructure.Configurations.Cart;

internal sealed class CartWriteConfiguration : IEntityTypeConfiguration<CartEntity>
{
    public void Configure(EntityTypeBuilder<CartEntity> builder)
    {
        builder.ToTable("Cart", "cart");

        builder.HasKey(c => c.Id);

        builder.HasQueryFilter(c => c.DeletedOn == null);

        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.Uid).ValueGeneratedNever();

        builder.HasIndex(c => c.Uid).IsUnique();

        builder.HasIndex(c => c.AnonymousKey).HasDatabaseName("IX_Cart_AnonymousKey");

        builder.Property(c => c.CreatedOn).HasColumnType("datetime2(0)").IsRequired();

        builder.Property(c => c.DeletedOn).HasColumnType("datetime2(0)");

        builder.Property(c => c.UserId);

        builder.Property(c => c.AnonymousKey).HasMaxLength(128);
    }
}
