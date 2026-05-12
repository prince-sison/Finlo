using System.Linq.Expressions;
using Finlo.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Finlo.Infrastructure.Persistence.Sql.Repositories;

public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
     where TEntity : class
     where TKey : IEquatable<TKey>
{
    private readonly FinloDbContext _context;

    public BaseRepository(FinloDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken).AsTask();
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
