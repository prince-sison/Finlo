using Finlo.Application.DTOs.Budgets;
using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Budgets.Commands.UpdateBudget;

public sealed record UpdateBudgetCommand(
    Guid Id,
    string Category,
    decimal Limit,
    int Month,
    int Year) : ICommand<BudgetResponseDto>;