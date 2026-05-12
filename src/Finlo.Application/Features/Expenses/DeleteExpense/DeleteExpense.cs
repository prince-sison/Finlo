using Finlo.Application.Common.Interfaces.Services;

namespace Finlo.Application.Features.Expenses.DeleteExpense;

public class DeleteExpense(IExpenseService expenseService)
{
    public async Task HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await expenseService.DeleteAsync(id, cancellationToken);
    }
}
