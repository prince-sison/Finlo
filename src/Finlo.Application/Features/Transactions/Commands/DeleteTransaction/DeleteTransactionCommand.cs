using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Transactions.Commands.DeleteTransaction;

public sealed record DeleteTransactionCommand(Guid Id) : ICommand;
