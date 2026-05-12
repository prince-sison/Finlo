using Finlo.Application.Common.Interfaces.Services;
using Finlo.Application.Dtos.Expense;

namespace Finlo.Application.Features.Expenses.CreateExpense;

public class CreateExpense(IExpenseService expenseService)
{
    public async Task HandleAsync(CreateExpenseDto dto, CancellationToken cancellationToken = default)
    {
        await expenseService.AddAsync(dto, cancellationToken);
    }
}