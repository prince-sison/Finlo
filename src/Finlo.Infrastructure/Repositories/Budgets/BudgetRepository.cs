using Finlo.Application.Interfaces.Budgets;
using Finlo.Domain.Entities;
using Finlo.Infrastructure.Data;

namespace Finlo.Infrastructure.Repositories.Budgets;

public class BudgetRepository : BaseRepository<Budget, Guid>, IBudgetRepository
{
    public BudgetRepository(AppDbContext context) : base(context)
    {
    }
}