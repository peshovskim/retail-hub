using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

internal sealed class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
    {
        builder.ToTable("AspNetUserLogins", "identity");
        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        builder.Property(l => l.LoginProvider).HasMaxLength(450);
        builder.Property(l => l.ProviderKey).HasMaxLength(450);
        builder.Property(l => l.ProviderDisplayName).HasColumnType("nvarchar(max)");
        builder.HasIndex(l => l.UserId).HasDatabaseName("IX_AspNetUserLogins_UserId");
    }
}
