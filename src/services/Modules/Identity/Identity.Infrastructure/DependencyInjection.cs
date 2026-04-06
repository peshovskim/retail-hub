using Identity.Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.SharedKernel.Application.Common.Abstractions;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<RetailHubIdentityDbContext>(options =>
            options.UseSqlServer(connectionString));

        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RetailHubIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IUserIdentityLookup, UserIdentityLookup>();

        return services;
    }
}
