namespace Finlo.Application.Dtos.Expense;

public class CreateExpenseDto
{
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }
}