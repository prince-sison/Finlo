using Finlo.Application.Common.Interfaces.Repositories;
using Finlo.Domain.Entities;

namespace Finlo.Infrastructure.Persistence.Sql.Repositories;

public class ExpenseRepository(FinloDbContext context)
    : BaseRepository<Expense, Guid>(context), IExpenseRepository
{
}
