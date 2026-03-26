using Finlo.Application.DTOs.Common;
using Finlo.Domain.Entities;

namespace Finlo.Application.Interfaces.Budgets;

public interface IBudgetRepository : IBaseRepository<Budget, Guid>
{
    Task<PagedResult<Budget>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default);
}