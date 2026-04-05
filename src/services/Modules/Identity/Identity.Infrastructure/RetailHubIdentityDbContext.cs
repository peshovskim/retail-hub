using Identity.Infrastructure.Configurations;
using Identity.Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;

public sealed class RetailHubIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public RetailHubIdentityDbContext(DbContextOptions<RetailHubIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationRoleConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityUserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityUserClaimConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityRoleClaimConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityUserLoginConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityUserTokenConfiguration());
    }
}
