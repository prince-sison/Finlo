using Finlo.Application.DTOs.Transactions;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Application.Interfaces.Transactions;
using Finlo.Domain.Entities;
using Finlo.Domain.Primitives;

namespace Finlo.Application.Features.Transactions.Commands.CreateTransaction;

internal sealed class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, TransactionResponseDto>
{
    private readonly ITransactionRepository _transactionRepository;

    public CreateTransactionCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<TransactionResponseDto>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = new Transaction
        {
            Amount = request.Amount,
            Type = request.Type,
            Category = request.Category,
            Date = request.Date,
            Notes = request.Notes
        };

        await _transactionRepository.AddAsync(transaction, cancellationToken);

        var response = new TransactionResponseDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Type = transaction.Type.ToString(),
            Category = transaction.Category,
            Date = transaction.Date,
            Notes = transaction.Notes
        };

        return Result.Success(response);
    }
}