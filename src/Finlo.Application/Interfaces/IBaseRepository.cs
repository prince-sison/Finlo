using Finlo.Application.DTOs.Common;

namespace Finlo.Application.Interfaces;

public interface IBaseRepository<TEntity, TKey>
    where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<PagedResult<TEntity>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
}