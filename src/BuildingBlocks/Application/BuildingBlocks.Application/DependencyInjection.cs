using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.BuildingBlocks.Application.Common.Abstractions.DomainEvents;
using RetailHub.BuildingBlocks.Application.Common.Behaviors;
using RetailHub.BuildingBlocks.Application.Common.DomainEvents;

namespace RetailHub.BuildingBlocks.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers building-blocks application services (domain event dispatcher). Register MediatR and validators from feature assemblies separately.
    /// </summary>
    public static IServiceCollection AddBuildingBlocksApplication(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
        return services;
    }

    /// <summary>
    /// Registers FluentValidation validators from the specified assemblies.
    /// </summary>
    public static IServiceCollection AddBuildingBlocksValidators(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
        {
            throw new ArgumentException("At least one assembly is required.", nameof(assemblies));
        }

        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }

    /// <summary>
    /// Adds shared MediatR pipeline behaviors (validation). Call from <c>AddMediatR</c> configuration.
    /// </summary>
    public static MediatRServiceConfiguration AddBuildingBlocksBehaviors(this MediatRServiceConfiguration configuration)
    {
        configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        return configuration;
    }
}
