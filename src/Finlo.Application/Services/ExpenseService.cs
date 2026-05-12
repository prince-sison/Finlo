using Finlo.Application.Common.Interfaces.Repositories;
using Finlo.Application.Common.Interfaces.Services;
using Finlo.Domain.Entities;

namespace Finlo.Application.Services;

public class ExpenseService(IExpenseRepository repository) : IExpenseService
{
    public Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => repository.GetByIdAsync(id, cancellationToken);

    public Task<IEnumerable<Expense>> GetAllAsync(CancellationToken cancellationToken = default)
        => repository.GetAllAsync(cancellationToken: cancellationToken);

    public Task AddAsync(Expense entity, CancellationToken cancellationToken = default)
        => repository.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(Expense entity, CancellationToken cancellationToken = default)
        => repository.UpdateAsync(entity, cancellationToken);

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var expense = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Expense with id '{id}' was not found.");

        await repository.DeleteAsync(expense, cancellationToken);
    }
}