using Finlo.Application.DTOs.Budgets;
using Finlo.Application.DTOs.Common;
using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Budgets.Queries.GetBudgetSummary;

public sealed record GetBudgetSummaryQuery(PaginationParams PaginationParams, int Month, int Year) : IQuery<BudgetSummaryDto>;
