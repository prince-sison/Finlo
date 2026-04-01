using Finlo.Application.Interfaces.Messaging;
using Finlo.Domain.Enums;
using MediatR;

namespace Finlo.Application.Features.Transactions.Commands.UpdateTransaction;

public sealed record UpdateTransactionCommand(
    Guid Id,
    decimal Amount,
    TransactionType Type,
    string Category,
    DateTime Date,
    string? Notes) : ICommand<Unit>;