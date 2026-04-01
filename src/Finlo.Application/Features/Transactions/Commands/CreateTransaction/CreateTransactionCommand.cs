using Finlo.Application.DTOs.Transactions;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Domain.Enums;

namespace Finlo.Application.Features.Transactions.Commands.CreateTransaction;

public sealed record CreateTransactionCommand(
    decimal Amount,
    TransactionType Type,
    string Category,
    DateTime Date,
    string? Notes) : ICommand<TransactionResponseDto>;