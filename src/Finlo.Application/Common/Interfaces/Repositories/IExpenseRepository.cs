using Finlo.Domain.Entities;

namespace Finlo.Application.Common.Interfaces.Repositories;

public interface IExpenseRepository : IBaseRepository<Expense, Guid>
{
}
