using Identity.Infrastructure.IdentityEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

internal sealed class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable("AspNetRoles", "identity");

        builder.Property(r => r.Name).HasMaxLength(256);
        builder.Property(r => r.NormalizedName).HasMaxLength(256).IsRequired();
        builder.Property(r => r.ConcurrencyStamp).HasColumnType("nvarchar(max)");
    }
}
