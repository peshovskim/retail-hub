using CategoryEntity = Catalog.Domain.Category.Domain.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Configurations.Category;

internal sealed class CategoryWriteConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("Category", "catalog");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.CreatedOn).HasColumnType("datetime2(0)").IsRequired();

        builder.Property(c => c.DeletedOn).HasColumnType("datetime2(0)");

        builder.Property(c => c.Name).HasMaxLength(256).IsRequired();

        builder.Property(c => c.Slug).HasMaxLength(256).IsRequired();

        builder.Property(c => c.ParentId);

        builder.HasOne<CategoryEntity>()
            .WithMany()
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
