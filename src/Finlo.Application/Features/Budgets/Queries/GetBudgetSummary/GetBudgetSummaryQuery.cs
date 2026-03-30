using Finlo.Application.DTOs.Budgets;
using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Budgets.Queries.GetBudgetSummary;

public sealed record GetBudgetSummaryQuery(int Month, int Year) : IQuery<BudgetSummaryDto>;
