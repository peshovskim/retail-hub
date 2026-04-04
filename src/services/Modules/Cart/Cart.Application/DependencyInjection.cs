using Cart.Application.Cart.Queries.GetCart;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.SharedKernel.Application;

namespace Cart.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCartApplication(this IServiceCollection services)
    {
        services.AddSharedKernelValidators(typeof(GetCartQuery).Assembly);
        return services;
    }
}
