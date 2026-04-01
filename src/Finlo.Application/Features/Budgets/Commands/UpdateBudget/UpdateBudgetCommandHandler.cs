using Finlo.Application.DTOs.Budgets;
using Finlo.Application.Interfaces;
using Finlo.Application.Interfaces.Budgets;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Domain.Primitives;

namespace Finlo.Application.Features.Budgets.Commands.UpdateBudget;

internal sealed class UpdateBudgetCommandHandler : ICommandHandler<UpdateBudgetCommand, BudgetResponseDto>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBudgetCommandHandler(IBudgetRepository budgetRepository, IUnitOfWork unitOfWork)
    {
        _budgetRepository = budgetRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BudgetResponseDto>> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = await _budgetRepository.GetByIdAsync(request.Id, cancellationToken);

        if (budget is null)
        {
            return Result.Failure<BudgetResponseDto>(Error.NotFound("Budget.NotFound", "Budget not found."));
        }

        budget.Category = request.Category;
        budget.Limit = request.Limit;
        budget.Month = request.Month;
        budget.Year = request.Year;
        budget.UpdatedAt = DateTime.UtcNow;

        await _budgetRepository.UpdateAsync(budget, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var budgetResponse = new BudgetResponseDto
        {
            Id = budget.Id,
            Category = budget.Category,
            Limit = budget.Limit,
            Month = budget.Month,
            Year = budget.Year
        };

        return Result.Success(budgetResponse);

    }
}