using Microsoft.Extensions.DependencyInjection;
using RetailHub.SharedKernel.Application;

namespace Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddSharedKernelValidators(typeof(DependencyInjection).Assembly);
        return services;
    }
}
