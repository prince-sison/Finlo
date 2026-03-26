namespace Finlo.Application.DTOs.Budgets;

public class UpdateBudgetDto
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public decimal Limit { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}