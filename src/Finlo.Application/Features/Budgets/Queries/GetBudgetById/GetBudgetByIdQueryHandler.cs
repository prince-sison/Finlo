using Finlo.Application.DTOs.Budgets;
using Finlo.Application.DTOs.Common;
using Finlo.Application.Interfaces.Budgets;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Domain.Primitives;

namespace Finlo.Application.Features.Budgets.Queries.GetBudgetById;

internal sealed class GetBudgetByIdQueryHandler : IQueryHandler<GetBudgetByIdQuery, PagedResult<BudgetResponseDto>>
{
    private readonly IBudgetRepository _budgetRepository;

    public GetBudgetByIdQueryHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public Task<Result<PagedResult<BudgetResponseDto>>> Handle(GetBudgetByIdQuery request, CancellationToken cancellationToken)
    {
        var budget = _budgetRepository.GetByIdAsync(request.Id, cancellationToken).Result;
        if (budget is null)
        {
            return Task.FromResult(Result.Failure<PagedResult<BudgetResponseDto>>(
                Error.NotFound("Budget.NotFound", $"Budget with ID '{request.Id}' was not found.")));
        }

        var response = new BudgetResponseDto
        {
            Id = budget.Id,
            Category = budget.Category,
            Limit = budget.Limit,
            Month = budget.Month,
            Year = budget.Year
        };

        var pagedResult = new PagedResult<BudgetResponseDto>
        {
            Items = [response],
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 1
        };

        return Task.FromResult(Result.Success(pagedResult));
    }
}