using Finlo.Application.DTOs.Common;
using Finlo.Application.Interfaces.Transactions;
using Finlo.Domain.Entities;
using Finlo.Domain.Enums;
using Finlo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Finlo.Infrastructure.Repositories.Transactions;

public class TransactionRepository : BaseRepository<Transaction, Guid>, ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedResult<Transaction>> GetFilteredAsync(
        PaginationParams paginationParams,
        TransactionType? type = null,
        string? category = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions.AsQueryable();

        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(t => t.Category == category);

        if (startDate.HasValue)
            query = query.Where(t => t.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(t => t.Date <= endDate.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Notes != null && t.Notes.Contains(search));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(t => t.Date)
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Transaction>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize
        };
    }
}