using Finlo.Domain.Entities;

namespace Finlo.Application.Interfaces.Transactions;

public interface ITransactionRepository : IBaseRepository<Transaction, Guid>
{
    
}