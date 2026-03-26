using Finlo.Application.DTOs.Common;
using Finlo.Application.DTOs.Transactions;
using Finlo.Application.Interfaces.Messaging;

namespace Finlo.Application.Features.Transactions.Queries.GetTransactionById;

public sealed record GetTransactionByIdQuery(Guid Id) : IQuery<PagedResult<TransactionResponseDto>>;