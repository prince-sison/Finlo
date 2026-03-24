using Finlo.Application.Interfaces.Transactions;
using Finlo.Domain.Entities;
using Finlo.Infrastructure.Data;

namespace Finlo.Infrastructure.Repositories.Transactions;

public class TransactionRepository : BaseRepository<Transaction, Guid>, ITransactionRepository
{
    public TransactionRepository(AppDbContext context) : base(context)
    {
    }
}