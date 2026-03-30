using Finlo.Application.DTOs.Common;
using Finlo.Domain.Entities;
using Finlo.Domain.Enums;

namespace Finlo.Application.Interfaces.Transactions;

public interface ITransactionRepository : IBaseRepository<Transaction, Guid>
{
    Task<PagedResult<Transaction>> GetFilteredAsync(
        PaginationParams paginationParams,
        TransactionType? type = null,
        string? category = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? search = null,
        CancellationToken cancellationToken = default);
    

}