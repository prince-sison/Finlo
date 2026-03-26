using Finlo.Application.DTOs.Budgets;
using Finlo.Application.DTOs.Common;
using Finlo.Application.Interfaces.Budgets;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Domain.Primitives;

namespace Finlo.Application.Features.Budgets.Queries.GetAllBudgets;

internal sealed class GetAllBudgetsQueryHandler : IQueryHandler<GetAllBudgetsQuery, PagedResult<BudgetResponseDto>>
{
    private readonly IBudgetRepository _budgetRepository;

    public GetAllBudgetsQueryHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<Result<PagedResult<BudgetResponseDto>>> Handle(GetAllBudgetsQuery request, CancellationToken cancellationToken)
    {
        var pagedBudgets = await _budgetRepository.GetAllAsync(request.PaginationParams, cancellationToken);

        var budgetDtos = pagedBudgets.Items.Select(budget => new BudgetResponseDto
        {
            Id = budget.Id,
            Category = budget.Category,
            Limit = budget.Limit,
            Month = budget.Month,
            Year = budget.Year,
            CreatedAt = budget.CreatedAt,
            UpdatedAt = budget.UpdatedAt
        }).ToList();

        var result = new PagedResult<BudgetResponseDto>
        {
            Items = budgetDtos,
            TotalCount = pagedBudgets.TotalCount,
            PageNumber = pagedBudgets.PageNumber,
            PageSize = pagedBudgets.PageSize
        };

        return Result.Success(result);
    }
}