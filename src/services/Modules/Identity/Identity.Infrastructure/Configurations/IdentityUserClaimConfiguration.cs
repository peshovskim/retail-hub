using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

internal sealed class IdentityUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.ToTable("AspNetUserClaims", "identity");
        builder.Property(c => c.Id).UseIdentityColumn();
        builder.HasIndex(c => c.UserId).HasDatabaseName("IX_AspNetUserClaims_UserId");
        builder.Property(c => c.ClaimType).HasColumnType("nvarchar(max)");
        builder.Property(c => c.ClaimValue).HasColumnType("nvarchar(max)");
    }
}
