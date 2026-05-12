using Finlo.Application.Common.Interfaces.Services;
using Finlo.Application.Dtos.Expense;
using Finlo.Domain.Entities;
using FluentValidation;

namespace Finlo.Application.Features.Expenses.CreateExpense;

public class CreateExpense(IExpenseService expenseService, IValidator<CreateExpenseDto> validator)
{
    public async Task<ExpenseResponseDto> HandleAsync(CreateExpenseDto dto, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(dto, cancellationToken);

        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Description = dto.Description,
            Amount = (long)dto.Amount,
            Date = dto.Date,
            CategoryId = dto.CategoryId
        };

        await expenseService.AddAsync(expense, cancellationToken);

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