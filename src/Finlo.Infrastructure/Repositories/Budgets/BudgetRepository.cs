using Finlo.Application.Interfaces.Budgets;
using Finlo.Domain.Entities;
using Finlo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Finlo.Infrastructure.Repositories.Budgets;

public class BudgetRepository : BaseRepository<Budget, Guid>, IBudgetRepository
{
    private readonly AppDbContext _context;

    public BudgetRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Budget>> GetByMonthAndYearAsync(int month, int year, CancellationToken cancellationToken = default)
    {
        return await _context.Budgets
            .Where(b => b.Month == month && b.Year == year)
            .ToListAsync(cancellationToken);
    }
}