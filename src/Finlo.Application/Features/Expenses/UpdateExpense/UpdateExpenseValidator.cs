using Finlo.Application.Dtos.Expense;
using FluentValidation;

namespace Finlo.Application.Features.Expenses.UpdateExpense;

public class UpdateExpenseValidator : AbstractValidator<UpdateExpenseDto>
{
    public UpdateExpenseValidator()
    {
        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Date)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}
