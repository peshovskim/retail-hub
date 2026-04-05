using Identity.Application.User.Commands.RegisterUser;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.SharedKernel.Application;

namespace Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddSharedKernelValidators(typeof(RegisterUserCommand).Assembly);
        return services;
    }
}
