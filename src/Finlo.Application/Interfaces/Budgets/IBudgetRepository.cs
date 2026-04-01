using Finlo.Domain.Entities;

namespace Finlo.Application.Interfaces.Budgets;

public interface IBudgetRepository : IBaseRepository<Budget, Guid>
{
    Task<List<Budget>> GetByMonthAndYearAsync(int month, int year, CancellationToken cancellationToken = default);
}