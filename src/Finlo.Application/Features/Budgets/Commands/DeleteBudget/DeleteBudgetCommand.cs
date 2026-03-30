using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Budgets.Commands.DeleteBudget;

public sealed record DeleteBudgetCommand(Guid Id) : ICommand;