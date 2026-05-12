namespace Finlo.Application.Dtos.Expense;

public class UpdateExpenseDto
{
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }
}