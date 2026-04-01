using Finlo.Application.DTOs.Budgets;
using Finlo.Application.Interfaces;
using Finlo.Application.Interfaces.Budgets;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Domain.Entities;
using Finlo.Domain.Primitives;

namespace Finlo.Application.Features.Budgets.Commands.CreateBudget;

internal sealed class CreateBudgetCommandHandler : ICommandHandler<CreateBudgetCommand, BudgetResponseDto>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBudgetCommandHandler(IBudgetRepository budgetRepository, IUnitOfWork unitOfWork)
    {
        _budgetRepository = budgetRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BudgetResponseDto>> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = new Budget
        {
            Category = request.Category,
            Limit = request.Limit,
            Month = request.Month,
            Year = request.Year,
            CreatedAt = DateTime.UtcNow
        };

        await _budgetRepository.AddAsync(budget, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var budgetResponse = new BudgetResponseDto
        {
            Id = budget.Id,
            Category = budget.Category,
            Limit = budget.Limit,
            Month = budget.Month,
            Year = budget.Year,
            CreatedAt = budget.CreatedAt
        };

        return Result.Success(budgetResponse);
    }
}