using CategoryEntity = Catalog.Domain.Category.Domain.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Configurations;

internal sealed class CategoryReadConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("Category", "catalog");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
        builder.Property(c => c.Slug).HasMaxLength(256).IsRequired();
    }
}
