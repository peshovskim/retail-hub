using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

internal sealed class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.ToTable("AspNetUserRoles", "identity");
        builder.HasKey(r => new { r.UserId, r.RoleId });
        builder.HasIndex(r => r.RoleId).HasDatabaseName("IX_AspNetUserRoles_RoleId");
    }
}
