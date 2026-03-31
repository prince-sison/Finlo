using Finlo.Application.DTOs.Budgets;
using Finlo.Application.Interfaces.Budgets;
using Finlo.Application.Interfaces.Messaging;
using Finlo.Application.Interfaces.Transactions;
using Finlo.Domain.Enums;
using Finlo.Domain.Primitives;

namespace Finlo.Application.Features.Budgets.Queries.GetBudgetSummary;

internal sealed class GetBudgetSummaryQueryHandler : IQueryHandler<GetBudgetSummaryQuery, BudgetSummaryDto>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ITransactionRepository _transactionRepository;

    public GetBudgetSummaryQueryHandler(IBudgetRepository budgetRepository, ITransactionRepository transactionRepository)
    {
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<BudgetSummaryDto>> Handle(GetBudgetSummaryQuery request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetRepository.GetByMonthAndYearAsync(request.Month, request.Year, cancellationToken);

        var startDate = new DateTime(request.Year, request.Month, 1);
        var endDate = startDate.AddMonths(1).AddTicks(-1);

        var expenseTransactions = await _transactionRepository.GetFilteredAsync(
            request.PaginationParams,
            type: TransactionType.Expense,
            startDate: startDate,
            endDate: endDate,
            cancellationToken: cancellationToken);

        var spentByCategory = expenseTransactions.Items
            .GroupBy(t => t.Category)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));

        var budgetItems = budgets.Select(b =>
        {
            var spent = spentByCategory.GetValueOrDefault(b.Category, 0m);
            var remaining = b.Limit - spent;
            var percentUsed = b.Limit > 0 ? Math.Round(spent / b.Limit * 100, 1) : 0;

            return new BudgetSummaryItemDto
            {
                Category = b.Category,
                Limit = b.Limit,
                Spent = spent,
                Remaining = remaining,
                PercentUsed = percentUsed
            };
        }).ToList();

        var summary = new BudgetSummaryDto
        {
            Month = request.Month,
            Year = request.Year,
            Budgets = budgetItems,
            TotalBudget = budgets.Sum(b => b.Limit),
            TotalSpent = budgetItems.Sum(b => b.Spent)
        };

        return Result.Success(summary);
    }
}
