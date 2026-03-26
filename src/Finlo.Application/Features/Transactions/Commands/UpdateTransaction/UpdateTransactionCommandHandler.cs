using Finlo.Application.Interfaces;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Application.Interfaces.Transactions;
using Finlo.Domain.Primitives;
using MediatR;

namespace Finlo.Application.Features.Transactions.Commands.UpdateTransaction;

internal sealed class UpdateTransactionCommandHandler : ICommandHandler<UpdateTransactionCommand, Unit>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (transaction is null)
        {
            return Result.Failure<Unit>(Error.NotFound("Transaction.NotFound", "Transaction not found."));
        }

        transaction.Amount = request.Amount;
        transaction.Type = request.Type;
        transaction.Category = request.Category;
        transaction.Date = request.Date;
        transaction.Notes = request.Notes;
        transaction.UpdatedAt = DateTime.UtcNow;
        await _transactionRepository.UpdateAsync(transaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}