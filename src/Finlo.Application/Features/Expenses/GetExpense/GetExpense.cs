using Finlo.Application.Common.Interfaces.Services;
using Finlo.Application.Dtos.Expense;

namespace Finlo.Application.Features.Expenses.GetExpense;

public class GetExpense(IExpenseService expenseService)
{
    public async Task<ExpenseResponseDto?> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var expense = await expenseService.GetByIdAsync(id, cancellationToken);

        if (expense is null)
            return null;

        return new ExpenseResponseDto
        {
            Id = expense.Id,
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date,
            CategoryId = expense.CategoryId
        };
    }
}
