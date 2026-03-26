using Finlo.Application.DTOs.Budgets;
using Finlo.Application.DTOs.Common;
using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Budgets.Queries.GetAllBudgets;

public sealed record GetAllBudgetsQuery(PaginationParams PaginationParams) : IQuery<PagedResult<BudgetResponseDto>>;