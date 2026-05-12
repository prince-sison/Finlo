using Finlo.Application.Dtos.Expense;
using FluentValidation;

namespace Finlo.Application.Features.Expenses.CreateExpense;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseDto>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Date)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}
