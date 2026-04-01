namespace Finlo.Application.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
