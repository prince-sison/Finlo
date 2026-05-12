using Finlo.Application.Common.Interfaces.Services;
using Finlo.Application.Dtos.Expense;

namespace Finlo.Application.Features.Expenses.GetAllExpenses;

public class GetAllExpenses(IExpenseService expenseService)
{
    public async Task<IEnumerable<ExpenseResponseDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var expenses = await expenseService.GetAllAsync(cancellationToken);

        return expenses.Select(e => new ExpenseResponseDto
        {
            Id = e.Id,
            Description = e.Description,
            Amount = e.Amount,
            Date = e.Date,
            CategoryId = e.CategoryId
        });
    }
}
