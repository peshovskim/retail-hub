using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.SharedKernel.Application.Common.Abstractions.DomainEvents;
using RetailHub.SharedKernel.Application.Common.Behaviors;
using RetailHub.SharedKernel.Application.Common.DomainEvents;

namespace RetailHub.SharedKernel.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers shared-kernel application services (domain event dispatcher). Register MediatR and validators from feature assemblies separately.
    /// </summary>
    public static IServiceCollection AddSharedKernelApplication(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
        return services;
    }

    /// <summary>
    /// Registers FluentValidation validators from the specified assemblies.
    /// </summary>
    public static IServiceCollection AddSharedKernelValidators(this IServiceCollection services, params Assembly[] assemblies)
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
    public static MediatRServiceConfiguration AddSharedKernelBehaviors(this MediatRServiceConfiguration configuration)
    {
        configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        return configuration;
    }
}
