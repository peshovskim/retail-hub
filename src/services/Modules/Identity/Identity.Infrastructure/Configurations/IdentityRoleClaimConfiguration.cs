using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

internal sealed class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
    {
        builder.ToTable("AspNetRoleClaims", "identity");
        builder.Property(c => c.Id).UseIdentityColumn();
        builder.HasIndex(c => c.RoleId).HasDatabaseName("IX_AspNetRoleClaims_RoleId");
        builder.Property(c => c.ClaimType).HasColumnType("nvarchar(max)");
        builder.Property(c => c.ClaimValue).HasColumnType("nvarchar(max)");
    }
}
