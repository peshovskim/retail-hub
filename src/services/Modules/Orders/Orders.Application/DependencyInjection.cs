using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Order.Commands.PlaceOrder;
using RetailHub.SharedKernel.Application;

namespace Orders.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOrdersApplication(this IServiceCollection services)
    {
        services.AddSharedKernelValidators(typeof(PlaceOrderCommand).Assembly);
        return services;
    }
}
