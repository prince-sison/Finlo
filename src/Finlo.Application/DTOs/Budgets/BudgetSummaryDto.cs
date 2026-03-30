namespace Finlo.Application.DTOs.Budgets;

public class BudgetSummaryDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public List<BudgetSummaryItemDto> Budgets { get; set; } = [];
    public decimal TotalBudget { get; set; }
    public decimal TotalSpent { get; set; }
}

public class BudgetSummaryItemDto
{
    public string Category { get; set; } = string.Empty;
    public decimal Limit { get; set; }
    public decimal Spent { get; set; }
    public decimal Remaining { get; set; }
    public decimal PercentUsed { get; set; }
}