using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

internal sealed class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
    {
        builder.ToTable("AspNetUserRoles", "identity");
        builder.HasKey(r => new { r.UserId, r.RoleId });
        builder.HasIndex(r => r.RoleId).HasDatabaseName("IX_AspNetUserRoles_RoleId");
    }
}
