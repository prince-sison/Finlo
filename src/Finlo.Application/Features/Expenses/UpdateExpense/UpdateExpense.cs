using Finlo.Application.Common.Interfaces.Services;
using Finlo.Application.Dtos.Expense;
using FluentValidation;

namespace Finlo.Application.Features.Expenses.UpdateExpense;

public class UpdateExpense(IExpenseService expenseService, IValidator<UpdateExpenseDto> validator)
{
    public async Task<ExpenseResponseDto> HandleAsync(Guid id, UpdateExpenseDto dto, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(dto, cancellationToken);
        
        var expense = await expenseService.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Expense with id '{id}' was not found.");

        expense.Description = dto.Description;
        expense.Amount = (long)dto.Amount;
        expense.Date = dto.Date;
        expense.CategoryId = dto.CategoryId;

        await expenseService.UpdateAsync(expense, cancellationToken);

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
