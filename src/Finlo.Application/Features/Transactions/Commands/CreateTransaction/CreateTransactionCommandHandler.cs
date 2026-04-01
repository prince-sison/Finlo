using Finlo.Application.DTOs.Transactions;
using Finlo.Application.Interfaces;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Application.Interfaces.Transactions;
using Finlo.Domain.Entities;
using Finlo.Domain.Primitives;

namespace Finlo.Application.Features.Transactions.Commands.CreateTransaction;

internal sealed class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, TransactionResponseDto>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TransactionResponseDto>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = new Transaction
        {
            Amount = request.Amount,
            Type = request.Type,
            Category = request.Category,
            Date = request.Date,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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