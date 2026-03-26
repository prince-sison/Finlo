using Finlo.Application.DTOs.Budgets;
using Finlo.Application.DTOs.Common;
using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Budgets.Queries.GetBudgetById;

public sealed record GetBudgetByIdQuery(Guid Id) : IQuery<PagedResult<BudgetResponseDto>>;