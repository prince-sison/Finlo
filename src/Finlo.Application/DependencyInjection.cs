using System.Reflection;
using Finlo.Application.Common.Interfaces.Services;
using Finlo.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Finlo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IExpenseService, ExpenseService>();

        // Auto-register all feature handlers under the Features namespace
        var featureTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                && t.Namespace is not null
                && t.Namespace.Contains(".Features."));

        foreach (var type in featureTypes)
        {
            services.AddScoped(type);
        }

        return services;
    }
}
