using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

internal sealed class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
    {
        builder.ToTable("AspNetUserTokens", "identity");
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        builder.Property(t => t.LoginProvider).HasMaxLength(450);
        builder.Property(t => t.Name).HasMaxLength(450);
        builder.Property(t => t.Value).HasColumnType("nvarchar(max)");
    }
}
