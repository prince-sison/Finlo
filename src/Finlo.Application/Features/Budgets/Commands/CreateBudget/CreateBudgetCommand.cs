using Finlo.Application.DTOs.Budgets;
using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Budgets.Commands.CreateBudget;

public sealed record CreateBudgetCommand(
    string Category,
    decimal Limit,
    int Month,
    int Year) : ICommand<BudgetResponseDto>;