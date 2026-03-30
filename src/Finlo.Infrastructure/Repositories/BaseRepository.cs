using Finlo.Application.DTOs.Common;
using Finlo.Application.Interfaces;
using Finlo.Infrastructure.Data;
using Finlo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Finlo.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity, TKey>
    : IBaseRepository<TEntity, TKey>
    where TEntity : class
{
    private readonly AppDbContext _context;

    protected BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<TEntity>().FindAsync([id], cancellationToken);
    }

    public virtual async Task<PagedResult<TEntity>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default)
    {
        return await _context.Set<TEntity>()
            .AsNoTracking()
            .ToPagedResultAsync(paginationParams, cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Update(entity);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }
}