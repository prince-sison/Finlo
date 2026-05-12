using Finlo.Domain.Entities;
using Finlo.Application.Dtos.Expense;
namespace Finlo.Application.Common.Interfaces.Services;

public interface IExpenseService
{
    Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Expense>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(CreateExpenseDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Expense entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
