using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductImageEntity = Catalog.Domain.Product.Domain.ProductImage;

namespace Catalog.Infrastructure.Configurations.Product;

internal sealed class ProductImageReadConfiguration : IEntityTypeConfiguration<ProductImageEntity>
{
    public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
    {
        builder.ToTable("ProductImage", "catalog");

        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.Id).ValueGeneratedOnAdd();

        builder.Property(pi => pi.Uid).ValueGeneratedNever();

        builder.Property(pi => pi.CreatedOn).HasColumnType("datetime2(0)").IsRequired();

        builder.Property(pi => pi.DeletedOn).HasColumnType("datetime2(0)");

        builder.HasIndex(pi => pi.Uid).IsUnique();

        builder.HasIndex(pi => pi.ProductId).HasDatabaseName("IX_ProductImage_ProductId");

        builder.Property(pi => pi.SortOrder).IsRequired();

        builder.Property(pi => pi.ImageUrl).HasMaxLength(2048).IsRequired();

        builder.HasOne(pi => pi.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
