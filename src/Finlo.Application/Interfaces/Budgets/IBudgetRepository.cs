using Finlo.Domain.Entities;

namespace Finlo.Application.Interfaces.Budgets;

public interface IBudgetRepository : IBaseRepository<Budget, Guid>
{
}