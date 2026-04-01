using Finlo.Application.DTOs.Common;
using Finlo.Application.DTOs.Transactions;
using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Transactions.Queries.GetAllTransactions;

public sealed record GetAllTransactionsQuery(PaginationParams PaginationParams) : IQuery<PagedResult<TransactionResponseDto>>;