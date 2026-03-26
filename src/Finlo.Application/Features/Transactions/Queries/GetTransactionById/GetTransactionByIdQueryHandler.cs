using Finlo.Application.DTOs.Common;
using Finlo.Application.DTOs.Transactions;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Application.Interfaces.Transactions;
using Finlo.Domain.Primitives;

namespace Finlo.Application.Features.Transactions.Queries.GetTransactionById;

internal sealed class GetTransactionByIdQueryHandler : IQueryHandler<GetTransactionByIdQuery, PagedResult<TransactionResponseDto>>
{
    private readonly ITransactionRepository _repository;

    public GetTransactionByIdQueryHandler(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedResult<TransactionResponseDto>>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (transaction is null)
        {
            return Result.Failure<PagedResult<TransactionResponseDto>>(
                Error.NotFound("Transaction.NotFound", $"Transaction with ID '{request.Id}' was not found."));
        }

        var response = new TransactionResponseDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Type = transaction.Type.ToString(),
            Category = transaction.Category,
            Date = transaction.Date,
            Notes = transaction.Notes
        };

        var pagedResult = new PagedResult<TransactionResponseDto>
        {
            Items = [response],
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 1
        };

        return Result.Success(pagedResult);
    }
}