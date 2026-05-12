using Finlo.Application.Common.Interfaces.Services;
using Finlo.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Finlo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IExpenseService, ExpenseService>();

        return services;
    }
}
