using Identity.Infrastructure.IdentityEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

internal sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("User", "identity");

        builder.Property(u => u.Uid).IsRequired();

        builder.HasIndex(u => u.Uid).IsUnique();

        builder.Property(u => u.CreatedOn).HasColumnType("datetime2(0)").IsRequired();

        builder.Property(u => u.UserName).HasMaxLength(256);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
        builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
        builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
        builder.Property(u => u.PhoneNumber).HasColumnType("nvarchar(max)");
        builder.Property(u => u.SecurityStamp).HasColumnType("nvarchar(max)");
        builder.Property(u => u.ConcurrencyStamp).HasColumnType("nvarchar(max)");
        builder.Property(u => u.PasswordHash).HasColumnType("nvarchar(max)");
    }
}
