using Finlo.Domain.Entities;

namespace Finlo.Application.Common.Interfaces.Services;

public interface IExpenseService
{
    Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Expense>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Expense entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Expense entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
