using Finlo.Application.DTOs.Common;
using Finlo.Application.DTOs.Transactions;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Application.Interfaces.Transactions;

namespace Finlo.Application.Features.Transactions.Queries.GetAllTransactions;

internal sealed class GetAllTransactionsQueryHandler : IQueryHandler<GetAllTransactionsQuery, PagedResult<TransactionResponseDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetAllTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Domain.Primitives.Result<PagedResult<TransactionResponseDto>>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var pagedTransactions = await _transactionRepository.GetAllAsync(request.PaginationParams, cancellationToken);

        var transactionDtos = pagedTransactions.Items.Select(t => new TransactionResponseDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Type = t.Type.ToString(),
            Category = t.Category,
            Date = t.Date,
            Notes = t.Notes,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        }).ToList();

        var result = new PagedResult<TransactionResponseDto>
        {
            Items = transactionDtos,
            TotalCount = pagedTransactions.TotalCount,
            PageNumber = pagedTransactions.PageNumber,
            PageSize = pagedTransactions.PageSize
        };

        return Domain.Primitives.Result.Success(result);
    }
}