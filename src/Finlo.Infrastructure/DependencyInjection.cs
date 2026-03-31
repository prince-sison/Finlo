using Finlo.Application.Interfaces;
using Finlo.Application.Interfaces.Budgets;
using Finlo.Application.Interfaces.Categories;
using Finlo.Application.Interfaces.Transactions;
using Finlo.Infrastructure.Data;
using Finlo.Infrastructure.Repositories;
using Finlo.Infrastructure.Repositories.Budgets;
using Finlo.Infrastructure.Repositories.Categories;
using Finlo.Infrastructure.Repositories.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finlo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}