using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductEntity = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Infrastructure.Configurations.Product;

internal sealed class ProductWriteConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("Product", "catalog");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.Uid).ValueGeneratedNever();

        builder.HasIndex(p => p.Uid).IsUnique();

        builder.HasIndex(p => p.CategoryId).HasDatabaseName("IX_Product_CategoryId");

        builder.Property(p => p.CreatedOn).HasColumnType("datetime2(0)").IsRequired();

        builder.Property(p => p.DeletedOn).HasColumnType("datetime2(0)");

        builder.Property(p => p.CategoryId).IsRequired();

        builder.Property(p => p.Name).HasMaxLength(256).IsRequired();

        builder.Property(p => p.Slug).HasMaxLength(256).IsRequired();

        builder.Property(p => p.Sku).HasMaxLength(64).IsRequired();

        builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(p => p.ShortDescription).HasMaxLength(512).IsRequired(false);

        builder.Property(p => p.Description).HasMaxLength(2000).IsRequired(false);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
